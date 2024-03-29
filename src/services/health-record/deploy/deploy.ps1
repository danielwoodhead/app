az account set --subscription <subscription_id>
# customize dockerignore to minimise the build context size
$dockerIgnorePath = "../../../.dockerignore"
$dockerIgnoreContent = "**`n!extensions`n!services/health-record`n**/.terraform`n**/.vs`n**/.bin`n**/.obj"
$dockerIgnoreContent | Set-Content $dockerIgnorePath

az acr build --registry myhealthregistry --image "myhealth/healthrecord.api`:latest" --file ../src/MyHealth.HealthRecord.Api/Dockerfile ../../..
terraform init -backend-config="resource_group_name=DansTerraform" -backend-config="storage_account_name=dansterraform" -backend-config="container_name=tfstate" -backend-config="key=dev.healthrecord.tfstate"
terraform validate
terraform apply -var-file="dev.tfvars"

# since we're not using different tags at this stage, restart the web app to pick up the new image
az webapp restart --name myhealth-healthrecord-api --resource-group DansApp

# delete untagged images
az acr repository show-manifests --name myhealthregistry --repository myhealth/healthrecord.api --query "[?tags[0]==null].digest" -o tsv `
  | ForEach-Object{ az acr repository delete --name myhealthregistry --image myhealth/healthrecord.api@$_ --yes }
