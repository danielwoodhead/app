az account set --subscription bc22730d-89ef-4562-9a78-dfb790976b9a
az acr build --registry myhealthregistry --image "myhealth/identity.admin`:latest" --file ../../src/MyHealth.Identity.Admin/Dockerfile ../..
terraform init -backend-config="resource_group_name=DansTerraform" -backend-config="storage_account_name=dansterraform" -backend-config="container_name=tfstate" -backend-config="key=dev.identity.admin.tfstate"
terraform validate

# on first run the app service has to be created before the sql firewall rules
terraform apply -target "azurerm_app_service.as" -var-file="dev.tfvars" -var identity_client_secret=secret

terraform apply -var-file="dev.tfvars" -var identity_client_secret=secret

# since we're not using different tags at this stage, restart the web app to pick up the new image
az webapp restart --name myhealth-identity-admin --resource-group DansApp

# delete untagged images
az acr repository show-manifests --name myhealthregistry --repository myhealth/identity.admin --query "[?tags[0]==null].digest" -o tsv `
  | ForEach-Object{ az acr repository delete --name myhealthregistry --image myhealth/identity.admin@$_ --yes }
