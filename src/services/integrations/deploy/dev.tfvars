resource_group_name = "DansApp"
app_service_plan_name = "myhealth-asp"
container_image_name = "myhealth/integrations.api:latest"
functions_container_image_name = "myhealth/integrations.funcs:latest"
container_registry_name = "myhealthregistry"
application_insights_name = "myhealth-insights"
authentication_authority = "https://myhealth-identity-sts.azurewebsites.net"
authentication_audience = "myhealth-integrations-api"
key_vault_name = "myhealth-keyvault"
iomt_event_hub_namespace = "myhealth-iomt-fhir"
iomt_event_hub_name = "devicedata"
iomt_event_hub_resource_group_name = "DansIomtFhir"
swagger_enabled = true
swagger_authorization_url = "https://myhealth-identity-sts.azurewebsites.net/connect/authorize"
swagger_token_url = "https://myhealth-identity-sts.azurewebsites.net/connect/token"
swagger_oauth_client_id = "myhealth-swaggerui"