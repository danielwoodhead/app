terraform {
  backend "azurerm" {}
}

provider "azurerm" {
  version = "=2.16.0"
  features {}
}

data "azurerm_sql_server" "sql" {
  name                = var.sql_server_name
  resource_group_name = var.resource_group_name
}

resource "azurerm_sql_database" "db" {
  name                = var.database_name
  resource_group_name = var.resource_group_name
  location            = var.location
  server_name         = data.azurerm_sql_server.sql.name
  edition             = var.sql_edition
}
