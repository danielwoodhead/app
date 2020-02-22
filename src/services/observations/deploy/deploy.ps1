az account set --subscription bc22730d-89ef-4562-9a78-dfb790976b9a
az acr build --registry myhealthregistry --image "myhealth/observations.api`:latest" --file ../src/MyHealth.Observations.Api/Dockerfile ../src
terraform validate
terraform init -backend-config="resource_group_name=DansTerraform" -backend-config="storage_account_name=dansterraform" -backend-config="container_name=tfstate" -backend-config="key=dev.observations.tfstate"
terraform apply -var-file="dev.tfvars"