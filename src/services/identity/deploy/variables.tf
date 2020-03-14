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
  default     = "Staging"
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

variable "database_name" {
  description = "The database name"
  default     = "MyHealthIdentity"
}

variable "sql_edition" {
  description = "The SQL edition e.g. Basic, Standard, Premium etc."
  default     = "Basic"
}

variable "key_vault_name" {
  description = "The name of the Azure Key Vault containing the SQL admin password"
}

variable "sql_admin_password_secret_name" {
  description = "The name of the SQL admin password secret in Azure Key Vault"
}

variable "identity_admin_email" {
  description =  "The identity admin user email"
}

variable "identity_admin_password" {
 description = "The identity admin user password" 
}
