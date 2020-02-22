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

variable "container_registry_sku" {
  description = "The container registry SKU"
  default     = "Basic"
}

variable "app_service_plan_tier" {
  description = "The app service plan tier"
  default     = "Basic"
}

variable "app_service_plan_size" {
  description = "The app serivce plan size"
  default     = "B1"
}