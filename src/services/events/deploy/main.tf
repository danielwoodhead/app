terraform {
  backend "azurerm" {}
  required_version = ">= 0.15"
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=2.58.0"
    }
  }
}

provider "azurerm" {
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

data "azurerm_eventgrid_topic" "topics" {
  for_each = { for topic in var.event_grid_topics : topic.identifier => topic }

  name                = each.value.name
  resource_group_name = each.value.resource_group_name
}

locals {
  topic_names = merge({ "Topics__0__Name" = "test" }, { for idx, val in var.event_grid_topics :
    "Topics__${idx + 1}__Name" => val.identifier
  })

  topic_uris = merge({ "Topics__0__Uri" = azurerm_eventgrid_topic.test.endpoint }, { for idx, val in var.event_grid_topics :
    "Topics__${idx + 1}__Uri" => data.azurerm_eventgrid_topic.topics[val.identifier].endpoint
  })

  topic_keys = merge({ "Topics__0__Key" = azurerm_eventgrid_topic.test.primary_access_key }, { for idx, val in var.event_grid_topics :
    "Topics__${idx + 1}__Key" => data.azurerm_eventgrid_topic.topics[val.identifier].primary_access_key
  })

  app_settings = merge({
    APPINSIGHTS_INSTRUMENTATIONKEY      = data.azurerm_application_insights.ai.instrumentation_key
    DOCKER_REGISTRY_SERVER_URL          = "https://${data.azurerm_container_registry.cr.login_server}"
    DOCKER_REGISTRY_SERVER_USERNAME     = data.azurerm_container_registry.cr.admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD     = data.azurerm_container_registry.cr.admin_password
    WEBSITES_ENABLE_APP_SERVICE_STORAGE = false
    Swagger__Enabled                    = var.swagger_enabled
    Swagger__AuthorizationUrl           = var.swagger_authorization_url
    Swagger__TokenUrl                   = var.swagger_token_url
    Swagger__OAuthClientId              = var.swagger_oauth_client_id
    BlobStorage__ConnectionString       = azurerm_storage_account.storage.primary_connection_string
  }, local.topic_names, local.topic_uris, local.topic_keys)
}

resource "azurerm_storage_account" "storage" {
  name                     = replace("${var.prefix}", "-", "")
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

  site_config {
    linux_fx_version = "DOCKER|${data.azurerm_container_registry.cr.login_server}/${var.container_image_name}"
  }

  app_settings = local.app_settings
}

resource "azurerm_eventgrid_topic" "test" {
  name                = "${var.prefix}-test-topic"
  location            = var.location
  resource_group_name = var.resource_group_name
  input_schema        = "CloudEventSchemaV1_0"
}

resource "azurerm_storage_queue" "test" {
  name                 = "${var.prefix}-test-queue"
  storage_account_name = azurerm_storage_account.storage.name
}

resource "azurerm_eventgrid_event_subscription" "test" {
  name                  = "${var.prefix}-test-subscription"
  scope                 = azurerm_eventgrid_topic.test.id
  event_delivery_schema = "CloudEventSchemaV1_0"

  storage_queue_endpoint {
    storage_account_id = azurerm_storage_account.storage.id
    queue_name         = azurerm_storage_queue.test.name
  }
}
