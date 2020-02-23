az acr helm repo add
helm install identity-api DansContainerRegistry/identity-api

# to pass in a value
#helm install identity-api DansContainerRegistry/identity-api --set image.repository=danscontainerregistry.azurecr.io/app/identity.api.identity_docker
