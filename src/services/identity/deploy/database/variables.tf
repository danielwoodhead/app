variable "resource_group_name" {
  description = "The name of the resource group"
}

variable "location" {
  description = "The location of all resources"
  default     = "uksouth"
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
