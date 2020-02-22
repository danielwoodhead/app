##########################################################################
# To allow the helm chart to pull the image from Azure Container Registry
##########################################################################

# 1. create docker-registry secret
kubectl create secret docker-registry dansregistrykey --docker-server=https://danscontainerregistry.azurecr.io --docker-username=DansContainerRegistry --docker-password=*** --docker-email=***

# 2. reference the secret in the helm chart
# imagePullSecrets:
#   - name: dansregistrykey


##########################################################################
# Use Azure Container Registry as a Helm Chart Repository
##########################################################################

az login
az configure --defaults acr=DansContainerRegistry
az acr helm repo add # also run this to authenticate Helm with ACR in the future

#########################################################################
# Helm commands
#########################################################################

# 1. Auth helm with ACR
az acr helm repo add

# 2. Create helm package
helm package .\identity-api

# 3. Push helm package to ACR
az acr helm push identity-api-0.1.0.tgz

# 4. To see the newly pushed chart
helm search repo DansContainerRegistry

# 5. Dry run install (local)
helm install identity-api --debug --dry-run ./identity-api

# 6. Real install (local)
helm install identity-api ./identity-api

# 7. Real install (ACR)
helm install identity-api DansContainerRegistry/identity-api

# 8. Uninstall
helm uninstall identity-api
