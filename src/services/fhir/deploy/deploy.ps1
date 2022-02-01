az account set --subscription <subscription_id>
az group create --location uksouth --name DansFhir
az deployment group create --resource-group DansFhir --template-file fhir-server/default-azuredeploy-sql.json --parameters default-azuredeploy-sql.parameters.json --parameters sqlAdminPassword={password}
