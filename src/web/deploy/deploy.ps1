az account set --subscription <subscription_id>
terraform init -backend-config="resource_group_name=DansTerraform" -backend-config="storage_account_name=dansterraform" -backend-config="container_name=tfstate" -backend-config="key=dev.app.web.tfstate"
terraform validate

terraform apply -var-file="dev.tfvars"

dotnet publish ../src/MyHealth.Web.App/MyHealth.Web.App.csproj -c Release

az storage blob upload-batch --source "../src/MyHealth.Web.App/bin/Release/net5.0/publish/wwwroot" --destination "`$web" --account-name "myhealthwebstorage"
