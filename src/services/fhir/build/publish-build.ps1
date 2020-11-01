az account set --subscription bc22730d-89ef-4562-9a78-dfb790976b9a

terraform init -backend-config="resource_group_name=DansTerraform" -backend-config="storage_account_name=dansterraform" -backend-config="container_name=tfstate" -backend-config="key=fhir.server.builds.tfstate"
terraform validate
terraform apply -var account_name=dansfhirbuilds -var container_name=builds

dotnet publish ../../../../../../os/fhir-server/src/Microsoft.Health.Fhir.R4.Web/Microsoft.Health.Fhir.R4.Web.csproj -c Release -o temp/build

# MSDeploy doesn't like powershell's Compress-Archive...
Add-Type -Assembly "System.IO.Compression.FileSystem" ;
[System.IO.Compression.ZipFile]::CreateFromDirectory("temp/build","temp/Microsoft.Health.Fhir.R4.Web.zip")

az storage blob upload --account-name dansfhirbuilds --container-name builds --file temp/Microsoft.Health.Fhir.R4.Web.zip --name Microsoft.Health.Fhir.R4.Web.zip

Remove-Item 'temp' -Recurse