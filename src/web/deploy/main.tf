terraform {
  backend "azurerm" {}
}

provider "azurerm" {
  version = "=2.17.0"
  features {}
}

resource "azurerm_storage_account" "example" {
  name                     = "myhealthwebstorage"
  resource_group_name      = var.resource_group_name
  location                 = var.location
  account_kind             = "StorageV2"
  account_tier             = "Standard"
  account_replication_type = "LRS"

  static_website {
    index_document     = "index.html"
    error_404_document = "index.html"
  }
}
