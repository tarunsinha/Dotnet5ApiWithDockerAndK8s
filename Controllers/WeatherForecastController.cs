using System;
// using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Dotnet5.Extensions;
using Microsoft.AspNetCore.Authorization;
// using Dotnet5.Authentication;
// using k8s;
// using Dotnet5.Models;

namespace dotnet5.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        IDistributedCache _cache;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching","Pleasant"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        // private readonly Kubernetes _client;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        //     _client = new Kubernetes(
        // KubernetesClientConfiguration.InClusterConfig());
        }

        [HttpGet]
        public async Task<WeatherForecastWrapper> Get()
        {
            // var pods = await _client.ListNamespacedPodAsync("default");
            // var thisPod = await _client.GetPo
            // var retVal = pods
            //   .Items
            //   .Select(p => new PodListModel
            //   {
            //       Name = p.Metadata.Name,
            //       Id = p.Metadata.Uid,
            //       NodeName = p.Spec.NodeName
            //   });

            var rng = new Random();
            string recordId = "WeatherForecast_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
            var forecasts = await _cache.GetRecordAsync<WeatherForecast[]>(recordId);
            var response = new WeatherForecastWrapper();
            if (forecasts is null)
            {
                forecasts = Enumerable.Range(1, 10).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)],
                })
                .ToArray();

                await _cache.SetRecordAsync(recordId, forecasts, null, TimeSpan.FromSeconds(10));
                response.weatherForecast = forecasts;
                response.Message = $"Loaded from API at {DateTime.Now}";
            }
            else
            {
                response.weatherForecast = forecasts;
                response.Message = $"Loaded from cache at {DateTime.Now}";
            }
            response.MachineName = Environment.MachineName;

            return response;
        }
    }
}