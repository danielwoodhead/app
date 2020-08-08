az account set --subscription bc22730d-89ef-4562-9a78-dfb790976b9a
az group create --location uksouth --name DansFhir
az deployment group create --resource-group DansFhir --template-file fhir-server/default-azuredeploy-sql.json --parameters default-azuredeploy-sql.parameters.json --parameters sqlAdminPassword={password}
