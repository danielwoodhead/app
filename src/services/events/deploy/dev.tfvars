resource_group_name       = "DansApp"
app_service_plan_name     = "myhealth-asp"
container_image_name      = "myhealth/events.api:latest"
container_registry_name   = "myhealthregistry"
application_insights_name = "myhealth-insights"
authentication_authority  = "https://myhealth-identity-sts.azurewebsites.net"
authentication_audience   = "myhealth-events-api"
swagger_enabled           = true
swagger_authorization_url = "https://myhealth-identity-sts.azurewebsites.net/connect/authorize"
swagger_token_url         = "https://myhealth-identity-sts.azurewebsites.net/connect/token"
swagger_oauth_client_id   = "myhealth-swaggerui"
#event_grid_topics = [
#  {
#    name                = "myhealth-integrations-event-topic"
#    identifier          = "integrations"
#    resource_group_name = "DansApp"
#  }
#]
