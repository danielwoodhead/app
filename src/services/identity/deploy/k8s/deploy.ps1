# auth helm with ACR
az acr helm repo add

helm install identity-api --debug --dry-run ./identity-api