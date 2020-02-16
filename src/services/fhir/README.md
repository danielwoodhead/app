# FHIR Server

This folder contains the scripts necessary to download the ARM templates for the Microsoft FHIR Server and deploy them.

## Known Issues

For some reason using the default templates cause a validation error with no useful detail. Also the UK regions aren't included as allowed locations for application insights.

The following changes have to be made to the default templates to remedy these issues:
1. Remove the nested template for the app service plan and just include the app service plan as a regular resource in the parent template
2. Adjust the 'depends on' value for the app service accordingly 
3. Add 'uksouth' as an allowed value for application insights location 