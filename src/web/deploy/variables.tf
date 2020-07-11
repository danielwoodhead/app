variable "resource_group_name" {
  description = "Resource group name"
}

variable "location" {
  description = "Resource location"
  default     = "uksouth"
}

variable "prefix" {
  description = "The prefix for all resource names"
  default     = "myhealth-web"
}
