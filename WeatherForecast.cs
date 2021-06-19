using System;
using System.Collections.Generic;

namespace dotnet5
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
        // public string Message { get; set; }
    }

    public class WeatherForecastWrapper
    {
        public WeatherForecast[] weatherForecast { get; set; }
        public string Message { get; set; }

        public string MachineName { get; set; }
    }
}
