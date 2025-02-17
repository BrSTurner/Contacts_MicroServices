@echo off

echo.
echo ========================================================
echo  Aplicando YAMLs no Kubernetes...
echo ========================================================
echo.

kubectl apply -f Source/Kubernetes/namespace.yaml

kubectl apply -f Source/Kubernetes/Services/services-configmap.yaml
kubectl apply -f Source/Kubernetes/Services/services-secret.yaml

kubectl apply -f Source/Kubernetes/PostgreSQL/postgresql-configmap.yaml
kubectl apply -f Source/Kubernetes/PostgreSQL/postgresql-secret.yaml
kubectl apply -f Source/Kubernetes/PostgreSQL/postgresql-pv.yaml
kubectl apply -f Source/Kubernetes/PostgreSQL/postgresql-pvc.yaml
kubectl apply -f Source/Kubernetes/PostgreSQL/postgresql-deployment.yaml
kubectl apply -f Source/Kubernetes/PostgreSQL/postgresql-service.yaml

kubectl apply -f Source/Kubernetes/RabbitMQ/rabbitmq-deployment.yaml
kubectl apply -f Source/Kubernetes/RabbitMQ/rabbitmq-service.yaml

kubectl apply -f Source/FIAP.DatabaseManagement.WS/database-management-deployment.yaml
kubectl apply -f Source/FIAP.DatabaseManagement.WS/database-management-service.yaml

kubectl apply -f Source/FIAP.Inquiry.WebAPI/inquiry-webapi-deployment.yaml
kubectl apply -f Source/FIAP.Inquiry.WebAPI/inquiry-webapi-service.yaml

kubectl apply -f Source/FIAP.Registration.WebAPI/registration-webapi-deployment.yaml
kubectl apply -f Source/FIAP.Registration.WebAPI/registration-webapi-service.yaml

kubectl apply -f Source/FIAP.Modification.WebAPI/modification-webapi-deployment.yaml
kubectl apply -f Source/FIAP.Modification.WebAPI/modification-webapi-service.yaml

kubectl apply -f Source/FIAP.Termination.WebAPI/termination-webapi-deployment.yaml
kubectl apply -f Source/FIAP.Termination.WebAPI/termination-webapi-service.yaml

kubectl rollout restart deployment -n fiap-contacts
kubectl rollout restart statefulset -n fiap-contacts

echo.
echo ========================================================
echo  Status dos pods
echo ========================================================
echo.

kubectl get pods -n fiap-contacts

echo.
echo Deploy concluido com sucesso
pause