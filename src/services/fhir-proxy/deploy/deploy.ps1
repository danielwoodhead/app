az account set --subscription <subscription_id>

az acr build --registry myhealthregistry --image "myhealth/fhir.proxy`:latest" --file ../src/MyHealth.Fhir.Proxy/Dockerfile ../src
terraform init -backend-config="resource_group_name=DansTerraform" -backend-config="storage_account_name=dansterraform" -backend-config="container_name=tfstate" -backend-config="key=dev.fhir.proxy.tfstate"
terraform validate
terraform apply -var-file="dev.tfvars"

# since we're not using different tags at this stage, restart the web apps to pick up the new image
az webapp restart --name myhealth-fhir-proxy --resource-group DansApp

# delete untagged images
az acr repository show-manifests --name myhealthregistry --repository myhealth/fhir.proxy --query "[?tags[0]==null].digest" -o tsv `
  | ForEach-Object{ az acr repository delete --name myhealthregistry --image myhealth/fhir.proxy@$_ --yes }
