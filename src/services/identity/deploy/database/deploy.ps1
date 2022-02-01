az account set --subscription <subscription_id>
terraform init -backend-config="resource_group_name=DansTerraform" -backend-config="storage_account_name=dansterraform" -backend-config="container_name=tfstate" -backend-config="key=dev.identity.database.tfstate"
terraform validate

terraform apply -var-file="dev.tfvars"
