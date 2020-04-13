terraform {
  backend "azurerm" {}
}

provider "azurerm" {
  version = "=1.44.0"
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

data "azurerm_key_vault" "kv" {
  name                = var.key_vault_name
  resource_group_name = var.resource_group_name
}

data "azurerm_key_vault_secret" "sql_admin_password" {
  name         = var.sql_admin_password_secret_name
  key_vault_id = data.azurerm_key_vault.kv.id
}

resource "azurerm_app_service" "as" {
  name                = "${var.prefix}-api"
  location            = var.location
  resource_group_name = var.resource_group_name
  app_service_plan_id = data.azurerm_app_service_plan.asp.id

  site_config {
    linux_fx_version = "DOCKER|${data.azurerm_container_registry.cr.login_server}/${var.container_image_name}"
  }

  identity {
    type = "SystemAssigned"
  }

  connection_string {
    name  = "DefaultConnection"
    type  = "SQLAzure"
    value = "Server=tcp:${data.azurerm_sql_server.sql.fqdn},1433;Initial Catalog=${azurerm_sql_database.db.name};Persist Security Info=False;User ID=${data.azurerm_sql_server.sql.administrator_login};Password=${data.azurerm_key_vault_secret.sql_admin_password.value};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }

  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY                  = data.azurerm_application_insights.ai.instrumentation_key
    ASPNETCORE_ENVIRONMENT                          = var.environment
    DOCKER_REGISTRY_SERVER_URL                      = "https://${data.azurerm_container_registry.cr.login_server}"
    DOCKER_REGISTRY_SERVER_USERNAME                 = data.azurerm_container_registry.cr.admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD                 = data.azurerm_container_registry.cr.admin_password
    WEBSITES_ENABLE_APP_SERVICE_STORAGE             = false
    Logging__ApplicationInsights__LogLevel__Default = var.log_level
    IdentityAdmin__Email                            = var.identity_admin_email
    IdentityServer__PublicOrigin                    = "https://${var.prefix}-api.azurewebsites.net"
    KeyVaultName                                    = data.azurerm_key_vault.kv.name
  }
}

resource "azurerm_sql_database" "db" {
  name                = var.database_name
  resource_group_name = var.resource_group_name
  location            = var.location
  server_name         = data.azurerm_sql_server.sql.name
  edition             = var.sql_edition
}

resource "azurerm_sql_firewall_rule" "sql-fw-rules" {
  count               = length(split(",", azurerm_app_service.as.possible_outbound_ip_addresses))
  name                = "${azurerm_app_service.as.name}_${count.index}"
  resource_group_name = var.resource_group_name
  server_name         = data.azurerm_sql_server.sql.name
  start_ip_address    = split(",", azurerm_app_service.as.possible_outbound_ip_addresses)[count.index]
  end_ip_address      = split(",", azurerm_app_service.as.possible_outbound_ip_addresses)[count.index]
}

resource "azurerm_key_vault_secret" "secret" {
  name         = "IdentityAdmin--Password"
  value        = var.identity_admin_password
  key_vault_id = data.azurerm_key_vault.kv.id
}

resource "azurerm_key_vault_access_policy" "kvp" {
  key_vault_id = data.azurerm_key_vault.kv.id

  tenant_id = azurerm_app_service.as.identity.0.tenant_id
  object_id = azurerm_app_service.as.identity.0.principal_id

  secret_permissions = [
    "get", "list"
  ]
}
