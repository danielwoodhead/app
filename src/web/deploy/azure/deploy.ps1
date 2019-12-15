az account set --subscription bc22730d-89ef-4562-9a78-dfb790976b9a
az group deployment create --resource-group DansMicroservices --template-file azuredeploy.json --parameters azuredeploy.parameters.json
az storage blob service-properties update --account-name dansappstorage --static-website --index-document index.html
az storage blob upload-batch --source ..\..\build --destination '$web' --account-name dansappstorage