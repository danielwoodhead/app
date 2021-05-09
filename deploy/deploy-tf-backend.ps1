az account set -s bc22730d-89ef-4562-9a78-dfb790976b9a
az group create --location uksouth --name DansTerraform
az storage account create -n dansterraform -g DansTerraform -l uksouth --kind StorageV2 --sku Standard_LRS
az storage container create -n tfstate --account-name dansterraform