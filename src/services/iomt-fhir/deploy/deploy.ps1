az account set --subscription bc22730d-89ef-4562-9a78-dfb790976b9a
az group create --location uksouth --name DansIomtFhir
az group deployment create --resource-group DansIomtFhir --template-file consumption-azuredeploy.json --parameters consumption-azuredeploy.parameters.json --parameters FhirServiceClientSecret={password}
