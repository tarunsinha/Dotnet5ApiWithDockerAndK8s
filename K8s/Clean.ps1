
kubectl delete service weather-api-db --ignore-not-found=true
kubectl delete service weather-api-svc --ignore-not-found=true
kubectl delete service weatherapi-redis --ignore-not-found=true
kubectl delete deployment redis-deployment --ignore-not-found=true
kubectl delete deployment mssql-deployment --ignore-not-found=true
kubectl delete deployment weather-api-deployment --ignore-not-found=true
kubectl delete ReplicaSet --all --ignore-not-found=true
kubectl delete pod --all --ignore-not-found=true
kubectl delete pvc,pv --all --ignore-not-found=true
kubectl delete secret mssql --ignore-not-found=true
clear