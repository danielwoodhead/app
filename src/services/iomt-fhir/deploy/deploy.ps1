az account set --subscription <subscription_id>
az group create --location uksouth --name DansIomtFhir
az deployment group create --resource-group DansIomtFhir --template-file iomt-fhir/consumption-azuredeploy.json --parameters consumption-azuredeploy.parameters.json --parameters FhirServiceClientSecret={password}
az storage blob upload-batch --source "../configuration" --destination "template" --account-name "myhealthiomtfhir"
