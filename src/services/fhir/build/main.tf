variable "account_name" {
}

variable "container_name" {
}

terraform {
  backend "azurerm" {}
}

provider "azurerm" {
  version = "=2.33.0"
  features {}
}

resource "azurerm_resource_group" "example" {
  name     = "DansFhirBuilds"
  location = "UK South"
}

resource "azurerm_storage_account" "example" {
  name                     = var.account_name
  resource_group_name      = azurerm_resource_group.example.name
  location                 = azurerm_resource_group.example.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  allow_blob_public_access = true
}

resource "azurerm_storage_container" "example" {
  name                  = var.container_name
  storage_account_name  = azurerm_storage_account.example.name
  container_access_type = "blob"
}
