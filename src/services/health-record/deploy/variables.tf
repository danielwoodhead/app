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

variable "fhir_server_base_address" {
  description = "The FHIR server base address"
}
