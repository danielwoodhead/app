variable "resource_group_name" {
  description = "The name of the resource group"
}

variable "location" {
  description = "The location of all resources"
  default     = "uksouth"
}

variable "prefix" {
  description = "The prefix for all resource names"
  default     = "myhealth-identity"
}

variable "environment" {
  description = "The ASPNETCORE_ENVIRONMENT environment variable"
  default     = "Development"
}

variable "app_service_plan_name" {
  description = "The app service plan name"
}

variable "container_image_name" {
  description = "The container image name"
}

variable "container_registry_name" {
  description = "The container registry name"
}

variable "application_insights_name" {
  description = "The Application Insights instance name"
}

variable "sql_server_name" {
  description = "The SQL Server name"
}

variable "sql_admin_password" {
  description = "The SQL Server admin password"
}

variable "database_name" {
  description = "The database name"
  default     = "MyHealthIdentity"
}

variable "sql_edition" {
  description = "The SQL edition e.g. Basic, Standard, Premium etc."
  default     = "Basic"
}