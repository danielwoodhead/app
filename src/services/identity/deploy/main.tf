provider "azurerm" {
  version = "=1.44.0"
}

terraform {
  backend "azurerm" {}
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

resource "azurerm_app_service" "as" {
  name                = "${var.prefix}-api"
  location            = var.location
  resource_group_name = var.resource_group_name
  app_service_plan_id = data.azurerm_app_service_plan.asp.id

  site_config {
    linux_fx_version = "DOCKER|${data.azurerm_container_registry.cr.login_server}/${var.container_image_name}"
  }

  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY       = data.azurerm_application_insights.ai.instrumentation_key
    ASPNETCORE_ENVIRONMENT               = var.environment
    DOCKER_REGISTRY_SERVER_URL           = "https://${data.azurerm_container_registry.cr.login_server}"
    DOCKER_REGISTRY_SERVER_USERNAME      = data.azurerm_container_registry.cr.admin_username
    DOCKER_REGISTRY_SERVER_PASSWORD      = data.azurerm_container_registry.cr.admin_password
    WEBSITES_ENABLE_APP_SERVICE_STORAGE  = false
    IdentityServer__PublicOrigin = "https://${var.prefix}-api.azurewebsites.net"
  }

  connection_string {
    name  = "DefaultConnection"
    type  = "SQLAzure"
    value = "Server=tcp:${data.azurerm_sql_server.sql.fqdn},1433;Initial Catalog=${azurerm_sql_database.db.name};Persist Security Info=False;User ID=${data.azurerm_sql_server.sql.administrator_login};Password=${var.sql_admin_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
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