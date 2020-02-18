# FHIR Server

This folder contains the scripts necessary to download the ARM templates for the Microsoft FHIR Server and deploy them. FHIR batch and transactions have to be enabled via feature flags (see default-azuredeploy-sql.parameters.json) and are only supported in the SQL version.

## Additional Changes

The following changes have been made to the default templates:
1. Add 'uksouth' as an allowed value for application insights location 
2. Change the SQL tier to Basic (5 DTUs)
3. The default templates currently fail to deploy due to https://github.com/microsoft/fhir-server/issues/877. A workaround is to replace the nested template for the app service plan with a regular resource in the parent template and adjust the 'depends on' value for the app service accordingly. 