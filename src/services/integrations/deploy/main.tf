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

data "azurerm_eventhub_namespace" "iomt" {
  name                = var.iomt_event_hub_namespace
  resource_group_name = var.iomt_event_hub_resource_group_name
}

resource "azurerm_storage_account" "storage" {
  name                     = replace("${var.prefix}00", "-", "")
  resource_group_name      = var.resource_group_name
  location                 = var.location
  account_kind             = "StorageV2"
  account_tier             = "Standard"
  account_replication_type = "LRS"
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
    ASPNETCORE_ENVIRONMENT                          = var.environment
    DOCKER_REGISTRY_SERVER_URL                      = "https://${data.azurerm_container_registry.cr.login_server}"
    DOCKER_REGISTRY_SERVER_USERNAME                 = data.azurerm_container_registry.cr.admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD                 = data.azurerm_container_registry.cr.admin_password
    WEBSITES_ENABLE_APP_SERVICE_STORAGE             = false
    Authentication__Authority                       = var.authentication_authority
    Authentication__Audience                        = var.authentication_audience
    EventGrid__Enabled                              = true
    EventGrid__TopicEndpoint                        = azurerm_eventgrid_topic.topic.endpoint
    EventGrid__TopicKey                             = azurerm_eventgrid_topic.topic.primary_access_key
    Fitbit__BaseUrl                                 = var.fitbit_base_url
    Fitbit__AuthenticationUrl                       = var.fitbit_authentication_url
    KeyVault__Name                                  = var.key_vault_name
    Logging__ApplicationInsights__LogLevel__Default = "Information"
    Swagger__Enabled                                = var.swagger_enabled
    Swagger__AuthorizationUrl                       = var.swagger_authorization_url
    Swagger__TokenUrl                               = var.swagger_token_url
    Swagger__OAuthClientId                          = var.swagger_oauth_client_id
  }

  connection_string {
    name  = "TableStorage"
    type  = "Custom"
    value = azurerm_storage_account.storage.primary_connection_string
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

resource "azurerm_function_app" "functions" {
  name                       = "${var.prefix}-funcs"
  location                   = var.location
  resource_group_name        = var.resource_group_name
  app_service_plan_id        = data.azurerm_app_service_plan.asp.id
  storage_account_name       = azurerm_storage_account.storage.name
  storage_account_access_key = azurerm_storage_account.storage.primary_access_key
  os_type                    = "linux"
  version                    = "~3"

  identity {
    type = "SystemAssigned"
  }

  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY      = data.azurerm_application_insights.ai.instrumentation_key
    DOCKER_REGISTRY_SERVER_URL          = "https://${data.azurerm_container_registry.cr.login_server}"
    DOCKER_REGISTRY_SERVER_USERNAME     = data.azurerm_container_registry.cr.admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD     = data.azurerm_container_registry.cr.admin_password
    WEBSITES_ENABLE_APP_SERVICE_STORAGE = false
    FUNCTIONS_WORKER_RUNTIME            = "dotnet"
    Fitbit__BaseUrl                     = var.fitbit_base_url
    Fitbit__VerificationCode            = "@Microsoft.KeyVault(SecretUri=${data.azurerm_key_vault.kv.vault_uri}secrets/Fitbit--VerificationCode/cfedf313627c40aca593ed84ef41e0ad)"
    Fitbit__ClientId                    = "@Microsoft.KeyVault(SecretUri=${data.azurerm_key_vault.kv.vault_uri}secrets/Fitbit--ClientId/45c7f90d003845af98100be07c951c3d)"
    Fitbit__ClientSecret                = "@Microsoft.KeyVault(SecretUri=${data.azurerm_key_vault.kv.vault_uri}secrets/Fitbit--ClientSecret/d425992ab34546029d8cffb83be6b887)"
    IoMT__EventHub__ConnectionString    = data.azurerm_eventhub_namespace.iomt.default_primary_connection_string
    IoMT__EventHub__Name                = var.iomt_event_hub_name
    KeyVault__Name                      = var.key_vault_name
  }

  connection_string {
    name  = "TableStorage"
    type  = "Custom"
    value = azurerm_storage_account.storage.primary_connection_string
  }

  site_config {
    always_on                 = true
    linux_fx_version          = "DOCKER|${data.azurerm_container_registry.cr.login_server}/${var.functions_container_image_name}"
    use_32_bit_worker_process = true
  }
}

resource "azurerm_key_vault_access_policy" "functions" {
  key_vault_id = data.azurerm_key_vault.kv.id
  tenant_id    = azurerm_function_app.functions.identity[0].tenant_id
  object_id    = azurerm_function_app.functions.identity[0].principal_id

  secret_permissions = [
    "get"
  ]
}

resource "azurerm_eventgrid_topic" "topic" {
  name                = "${var.prefix}-event-topic"
  location            = var.location
  resource_group_name = var.resource_group_name
}

resource "azurerm_eventgrid_event_subscription" "sub" {
  name  = "${var.prefix}-provider-update-event-subscription"
  scope = azurerm_eventgrid_topic.topic.id

  azure_function_endpoint {
    function_id                       = "${azurerm_function_app.functions.id}/functions/${var.function_name_integration_event}"
    max_events_per_batch              = 1 # default (added to prevent constant TF changes)
    preferred_batch_size_in_kilobytes = 64 # default (added to prevent constant TF changes)
  }
}
