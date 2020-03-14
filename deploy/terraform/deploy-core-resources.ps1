az account set -s bc22730d-89ef-4562-9a78-dfb790976b9a
terraform init -backend-config="resource_group_name=DansTerraform" -backend-config="storage_account_name=dansterraform" -backend-config="container_name=tfstate" -backend-config="key=dev.core.tfstate"
terraform validate
terraform apply -var-file="dev.tfvars" -var sql_admin_password={password}
