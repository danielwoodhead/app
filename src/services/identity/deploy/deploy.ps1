az account set --subscription bc22730d-89ef-4562-9a78-dfb790976b9a
az acr build --registry myhealthregistry --image "myhealth/identity.api`:latest" --file ../src/MyHealth.Identity.Api/Dockerfile ../src
terraform init -backend-config="resource_group_name=DansTerraform" -backend-config="storage_account_name=dansterraform" -backend-config="container_name=tfstate" -backend-config="key=dev.identity.tfstate"
terraform validate
terraform apply -var-file="dev.tfvars" -var sql_admin_password={password}

# since we're not using different tags at this stage, restart the web app to pick up the new image
az webapp restart --name myhealth-identity-api --resource-group DansApp
