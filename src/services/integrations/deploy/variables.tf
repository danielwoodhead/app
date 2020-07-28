variable "resource_group_name" {
  description = "The name of the resource group"
}

variable "location" {
  description = "The location of all resources"
  default     = "uksouth"
}

variable "prefix" {
  description = "The prefix for all resource names"
  default     = "myhealth-integrations"
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

variable "functions_container_image_name" {
  description = "The functions container image name"
}

variable "container_registry_name" {
  description = "The container registry name"
}

variable "application_insights_name" {
  description = "The Application Insights instance name"
}

variable "authentication_authority" {
  description = "The authentication authority"
}

variable "authentication_audience" {
  description = "The authentication audience"
}

variable "fitbit_base_url" {
  description = "The Fitbit API base URL"
  default     = "https://api.fitbit.com" 
}

variable "key_vault_name" {
  description = "The key vault name"
}

variable "integrations_table_name" {
  description = "The integrations table name"
  default     = "Integrations"
}