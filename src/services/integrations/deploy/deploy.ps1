az account set --subscription bc22730d-89ef-4562-9a78-dfb790976b9a

# customize dockerignore to minimise the build context size
$dockerIgnorePath = "../../../.dockerignore"
$dockerIgnoreContent = "**`n!extensions`n!services/integrations`n**/.terraform`n**/.vs`n**/.bin`n**/.obj"
$dockerIgnoreContent | Set-Content $dockerIgnorePath

az acr build --registry myhealthregistry --image "myhealth/integrations.api`:latest" --file ../src/MyHealth.Integrations.Api/Dockerfile ../../..
az acr build --registry myhealthregistry --image "myhealth/integrations.funcs`:latest" --file ../src/MyHealth.Integrations.FunctionApp/Dockerfile ../../..
terraform init -backend-config="resource_group_name=DansTerraform" -backend-config="storage_account_name=dansterraform" -backend-config="container_name=tfstate" -backend-config="key=dev.integrations.tfstate"
terraform validate
terraform apply -var-file="dev.tfvars"

# since we're not using different tags at this stage, restart the web apps to pick up the new image
az webapp restart --name myhealth-integrations-api --resource-group DansApp
az webapp restart --name myhealth-integrations-funcs --resource-group DansApp

# delete untagged images
az acr repository show-manifests --name myhealthregistry --repository myhealth/integrations.api --query "[?tags[0]==null].digest" -o tsv `
  | ForEach-Object{ az acr repository delete --name myhealthregistry --image myhealth/integrations.api@$_ --yes }

az acr repository show-manifests --name myhealthregistry --repository myhealth/integrations.funcs --query "[?tags[0]==null].digest" -o tsv `
  | ForEach-Object{ az acr repository delete --name myhealthregistry --image myhealth/integrations.funcs@$_ --yes }
