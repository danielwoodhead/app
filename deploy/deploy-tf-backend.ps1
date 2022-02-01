az account set -s <subscription_id>
az group create --location uksouth --name DansTerraform
az storage account create -n dansterraform -g DansTerraform -l uksouth --kind StorageV2 --sku Standard_LRS
az storage container create -n tfstate --account-name dansterraform