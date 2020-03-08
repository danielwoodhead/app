terraform {
  backend "azurerm" {}
}

provider "azurerm" {
  version = "=1.44.0"
}

data "azurerm_client_config" "current" {}

data "azuread_user" "admin" {
  user_principal_name = var.admin_user
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

resource "azurerm_container_registry" "cr" {
  name                = "${var.prefix}registry"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku                 = var.container_registry_sku
  admin_enabled       = true
}

resource "azurerm_key_vault" "kv" {
  name                        = "${var.prefix}-keyvault"
  location                    = azurerm_resource_group.rg.location
  resource_group_name         = azurerm_resource_group.rg.name
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  sku_name                    = var.key_vault_sku
}

resource "azurerm_key_vault_access_policy" "kvp1" {
  key_vault_id = azurerm_key_vault.kv.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azuread_user.admin.id

  certificate_permissions = [
    "backup", "create", "delete", "deleteissuers", "get", 
    "getissuers", "import", "list", "listissuers", "managecontacts", 
    "manageissuers", "purge", "recover", "restore", "setissuers", 
    "update"
  ]

  key_permissions = [
    "backup", "create", "decrypt", "delete", "encrypt", "get", 
    "import", "list", "purge", "recover", "restore", "sign", 
    "unwrapKey", "update", "verify", "wrapKey"
  ]

  secret_permissions = [
    "backup", "delete", "get", "list", "purge", "recover", "restore",
    "set"
  ]
}

resource "azurerm_key_vault_access_policy" "kvp2" {
  key_vault_id = azurerm_key_vault.kv.id

  tenant_id = data.azurerm_client_config.current.tenant_id
  object_id = data.azurerm_client_config.current.object_id

  secret_permissions = [
    "get", "list", "set", "delete"
  ]
}

resource "azurerm_key_vault_secret" "secret" {
  name         = "SqlAdminPassword"
  value        = var.sql_admin_password
  key_vault_id = azurerm_key_vault.kv.id
}

resource "azurerm_application_insights" "ai" {
  name                = "${var.prefix}-insights"
  location            = var.location
  resource_group_name = azurerm_resource_group.rg.name
  application_type    = "web"
}

resource "azurerm_app_service_plan" "asp" {
  name                = "${var.prefix}-asp"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  kind                = "Linux"
  reserved            = true

  sku {
    tier = var.app_service_plan_tier
    size = var.app_service_plan_size
  }
}

resource "azurerm_sql_server" "sql" {
  name                         = "${var.prefix}-sqlserver"
  resource_group_name          = var.resource_group_name
  location                     = var.location
  version                      = var.sql_server_version
  administrator_login          = var.sql_admin_username
  administrator_login_password = var.sql_admin_password
}
