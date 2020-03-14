variable "resource_group_name" {
  description = "The name of the resource group"
}

variable "location" {
  description = "The location of all resources"
  default     = "uksouth"
}

variable "prefix" {
  description = "The prefix for all resource names"
  default     = "myhealth"
}

variable "admin_user" {
  description = "The admin user"
}


variable "container_registry_sku" {
  description = "The container registry SKU"
  default     = "Basic"
}

variable "key_vault_sku" {
  description = "The key vault pricing tier"
  default     = "standard"
}

variable "app_service_plan_tier" {
  description = "The app service plan tier"
  default     = "Basic"
}

variable "app_service_plan_size" {
  description = "The app serivce plan size"
  default     = "B1"
}

variable "sql_server_version" {
  description = "The SQL Server version"
  default     = "12.0"
}

variable "sql_admin_username" {
  description = "The SQL admin username"
}

variable "sql_admin_password" {
  description = "The SQL admin password"
}
