terraform {
  backend "azurerm" {}
}

provider "azurerm" {
  version = "=2.22.0"
  features {}
}

data "azurerm_app_service_plan" "asp" {
  name                = var.app_service_plan_name
  resource_group_name = var.resource_group_name
}

data "azurerm_container_registry" "cr" {
  name                = var.container_registry_name
  resource_group_name = var.resource_group_name
}

data "azurerm_application_insights" "ai" {
  name                = var.application_insights_name
  resource_group_name = var.resource_group_name
}

data "azurerm_key_vault" "kv" {
  name                = var.key_vault_name
  resource_group_name = var.resource_group_name
}

resource "azurerm_app_service" "as" {
  name                = "${var.prefix}-api"
  location            = var.location
  resource_group_name = var.resource_group_name
  app_service_plan_id = data.azurerm_app_service_plan.asp.id

  identity {
    type = "SystemAssigned"
  }

  site_config {
    linux_fx_version = "DOCKER|${data.azurerm_container_registry.cr.login_server}/${var.container_image_name}"
  }

  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY                  = data.azurerm_application_insights.ai.instrumentation_key
    DOCKER_REGISTRY_SERVER_URL                      = "https://${data.azurerm_container_registry.cr.login_server}"
    DOCKER_REGISTRY_SERVER_USERNAME                 = data.azurerm_container_registry.cr.admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD                 = data.azurerm_container_registry.cr.admin_password
    WEBSITES_ENABLE_APP_SERVICE_STORAGE             = false
    Authentication__Authority                       = var.authentication_authority
    Authentication__Audience                        = var.authentication_audience
    Cors__AllowedOrigins__0                         = var.front_end_origin
    Cors__AllowedOrigins__1                         = var.front_end_origin_local
    HealthRecordApi__BaseAddress                    = var.health_record_api_base_address
    IdentityApi__TokenEndpoint                      = var.identity_api_token_endpoint
    IntegrationsApi__BaseAddress                    = var.integrations_api_base_address
    KeyVault__Name                                  = var.key_vault_name
    Logging__ApplicationInsights__LogLevel__Default = "Information"
    MyHealthAppApi__ClientScopes                    = var.my_health_app_api_client_scopes
  }
}

resource "azurerm_key_vault_access_policy" "api" {
  key_vault_id = data.azurerm_key_vault.kv.id
  tenant_id    = azurerm_app_service.as.identity[0].tenant_id
  object_id    = azurerm_app_service.as.identity[0].principal_id

  secret_permissions = [
    "get", "list"
  ]
}
