terraform {
  backend "azurerm" {}
}

provider "azurerm" {
  version = "=2.16.0"
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

data "azurerm_sql_server" "sql" {
  name                = var.sql_server_name
  resource_group_name = var.resource_group_name
}

data "azurerm_sql_database" "db" {
  name                = var.database_name
  server_name         = data.azurerm_sql_server.sql.name
  resource_group_name = var.resource_group_name
}

data "azurerm_key_vault" "kv" {
  name                = var.key_vault_name
  resource_group_name = var.resource_group_name
}

data "azurerm_key_vault_secret" "sql_admin_password" {
  name         = var.sql_admin_password_secret_name
  key_vault_id = data.azurerm_key_vault.kv.id
}

locals {
  app_service_name  = "${var.prefix}-sts"
  connection_string = "Server=tcp:${data.azurerm_sql_server.sql.fqdn},1433;Database=${data.azurerm_sql_database.db.name};User Id=${data.azurerm_sql_server.sql.administrator_login};Password=${data.azurerm_key_vault_secret.sql_admin_password.value};MultipleActiveResultSets=true"
}

resource "azurerm_app_service" "as" {
  name                = local.app_service_name
  location            = var.location
  resource_group_name = var.resource_group_name
  app_service_plan_id = data.azurerm_app_service_plan.asp.id

  site_config {
    linux_fx_version = "DOCKER|${data.azurerm_container_registry.cr.login_server}/${var.container_image_name}"
  }

  identity {
    type = "SystemAssigned"
  }

  dynamic "connection_string" {
    for_each = toset(["ConfigurationDbConnection", "PersistedGrantDbConnection", "IdentityDbConnection", "DataProtectionDbConnection"])

    content {
      name  = connection_string.value
      type  = "SQLAzure"
      value = local.connection_string
    }
  }

  app_settings = {
    DOCKER_REGISTRY_SERVER_URL               = "https://${data.azurerm_container_registry.cr.login_server}"
    DOCKER_REGISTRY_SERVER_USERNAME          = data.azurerm_container_registry.cr.admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD          = data.azurerm_container_registry.cr.admin_password
    WEBSITES_ENABLE_APP_SERVICE_STORAGE      = false
    APPINSIGHTS_INSTRUMENTATIONKEY           = data.azurerm_application_insights.ai.instrumentation_key
    AdminConfiguration__IdentityAdminBaseUrl = var.identity_admin_base_url
  }
}

resource "azurerm_sql_firewall_rule" "sql-fw-rules" {
  count               = length(split(",", azurerm_app_service.as.possible_outbound_ip_addresses))
  name                = "${azurerm_app_service.as.name}_${count.index}"
  resource_group_name = var.resource_group_name
  server_name         = data.azurerm_sql_server.sql.name
  start_ip_address    = split(",", azurerm_app_service.as.possible_outbound_ip_addresses)[count.index]
  end_ip_address      = split(",", azurerm_app_service.as.possible_outbound_ip_addresses)[count.index]
}
