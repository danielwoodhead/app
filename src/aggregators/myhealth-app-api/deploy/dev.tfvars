resource_group_name             = "DansApp"
app_service_plan_name           = "myhealth-asp"
container_image_name            = "myhealth/app.api:latest"
container_registry_name         = "myhealthregistry"
application_insights_name       = "myhealth-insights"
authentication_authority        = "https://myhealth-identity-sts.azurewebsites.net"
authentication_audience         = "myhealth-app-api"
fhir_api_address                = "https://myhealth-fhir.azurewebsites.net"
identity_api_base_address       = "https://myhealth-identity-admin-api.azurewebsites.net/api/"
identity_api_token_endpoint     = "https://myhealth-identity-sts.azurewebsites.net/connect/token"
integrations_api_base_address   = "https://myhealth-integrations-api.azurewebsites.net/v1/"
key_vault_name                  = "myhealth-keyvault"
my_health_app_api_client_scopes = "myhealth-healthrecord-api myhealth-identity-api myhealth-integrations-api fhir-api"
front_end_origin                = "https://myhealthwebstorage.z33.web.core.windows.net"
front_end_origin_local          = "https://localhost:44315"
swagger_enabled                 = true
swagger_authorization_url       = "https://myhealth-identity-sts.azurewebsites.net/connect/authorize"
swagger_token_url               = "https://myhealth-identity-sts.azurewebsites.net/connect/token"
swagger_oauth_client_id         = "myhealth-swaggerui"
