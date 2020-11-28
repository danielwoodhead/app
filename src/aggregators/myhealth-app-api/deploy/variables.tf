variable "resource_group_name" {
  description = "The name of the resource group"
}

variable "location" {
  description = "The location of all resources"
  default     = "uksouth"
}

variable "prefix" {
  description = "The prefix for all resource names"
  default     = "myhealth-app"
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

variable "key_vault_name" {
  description = "The key vault name"
}

variable "health_record_api_base_address" {
  description = "The Health Record API base address"
}

variable "identity_api_base_address" {
  description = "The Identity API base address"
}

variable "identity_api_token_endpoint" {
  description = "The Identity API token endpoint"
}

variable "integrations_api_base_address" {
  description = "The Integrations API base address"
}

variable "my_health_app_api_client_scopes" {
  description = "The MyHealth App API client scopes"
}

variable "front_end_origin" {
  description = "Origin of front-end application"
}

variable "front_end_origin_local" {
  description = "Origin of front-end application for local development"
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
