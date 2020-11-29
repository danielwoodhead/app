variable "resource_group_name" {
  description = "The name of the resource group"
}

variable "location" {
  description = "The location of all resources"
  default     = "uksouth"
}

variable "prefix" {
  description = "The prefix for all resource names"
  default     = "myhealth-healthrecord"
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

variable "authentication_authority" {
  description = "The authentication authority"
}

variable "authentication_audience" {
  description = "The authentication audience"
}

variable "fhir_server_base_url" {
  description = "The FHIR server base URL"
}

variable "fhir_server_timeout" {
  description = "The FHIR server timeout"
  default = "00:00:30"
}

variable "swagger_enabled" {
  description = "Should Swagger be enabled?"
  type = bool
}

variable "swagger_authorization_url" {
  description = "The authorization URL used by Swagger"
}

variable "swagger_token_url" {
  description = "The token URL used by Swagger"
}

variable "swagger_oauth_client_id" {
  description = "The OAuth client ID used by Swagger"
}
