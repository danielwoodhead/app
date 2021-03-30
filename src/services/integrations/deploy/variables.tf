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

variable "fhir_base_url" {
  description = "The FHIR API base URL"
}

variable "fhir_timeout" {
  description = "The FHIR API timeout"
  default = "00:00:30"
}

variable "authentication_client_id" {
  description = "The authentication client ID"
  default     = "myhealth-integrations-api" 
}

variable "authentication_scope" {
  description = "The authentication scope"
  default     = "fhir-api" 
}

variable "authentication_token_endpoint" {
  description = "The authentication token endpoint"
  default     = "https://myhealth-identity-sts.azurewebsites.net/connect/token" 
}

variable "fitbit_base_url" {
  description = "The Fitbit API base URL"
  default     = "https://api.fitbit.com" 
}

variable "fitbit_authentication_url" {
  description = "The Fitbit authentication URL"
  default     = "https://www.fitbit.com/oauth2/authorize" 
}

variable "key_vault_name" {
  description = "The key vault name"
}

variable "iomt_event_hub_namespace" {
  description = "The namespace of the IoMT event hub"
}

variable "iomt_event_hub_name" {
  description = "The name of the IoMT event hub"
}

variable "iomt_event_hub_resource_group_name" {
  description = "The name of the IoMT event hub resource group"
}

variable "strava_api_url" {
  description = "The Strava API URL"
  default     = "https://www.strava.com/api/v3/"
}

variable "strava_authentication_url" {
  description = "The Strava authentication URL"
  default     = "https://www.strava.com/oauth/authorize"
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
