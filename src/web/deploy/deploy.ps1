az account set --subscription bc22730d-89ef-4562-9a78-dfb790976b9a
terraform init -backend-config="resource_group_name=DansTerraform" -backend-config="storage_account_name=dansterraform" -backend-config="container_name=tfstate" -backend-config="key=dev.identity.tfstate"
terraform validate

terraform apply -var-file="dev.tfvars"

dotnet publish ../src/MyHealth.Web.App/MyHealth.Web.App.csproj -c Release

az storage blob upload-batch --source "../src/MyHealth.Web.App/bin/Release/netstandard2.1/publish/wwwroot" --destination "`$web" --account-name "myhealthwebstorage"
