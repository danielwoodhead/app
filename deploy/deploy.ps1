Param(
    [parameter(Mandatory=$true)][string]$subscription,
    [parameter(Mandatory=$true)][string]$resourceGroupName,
    [parameter(Mandatory=$true)][string]$location,
    [parameter(Mandatory=$true)][string]$serviceName,
    [parameter(Mandatory=$true)][string]$dnsNamePrefix,
    [parameter(Mandatory=$false)][string]$registryName,
    [parameter(Mandatory=$true)][bool]$createAcr=$true,
    [parameter(Mandatory=$false)][int]$nodeCount=3,
    [parameter(Mandatory=$false)][string]$nodeVMSize="Standard_D2_v2",
    [parameter(Mandatory=$false)][bool]$enableHttpApplicationAddon=$true,
    [parameter(Mandatory=$false)][bool]$enableAzureMonitoring=$false,
    [parameter(Mandatory=$false)][ValidateSet("VirtualMachineScaleSets","AvailabilitySet",IgnoreCase=$true)]$vmSetType="VirtualMachineScaleSets"
)

# Set subscription
Write-Host "Setting subscription..." -ForegroundColor Yellow
az account set --subscription $subscription

# Create resource group
Write-Host "Creating Azure Resource Group..." -ForegroundColor Yellow
az group create --name=$resourceGroupName --location=$location

if ($createAcr -eq $true) {
    # Create Azure Container Registry
    if ([string]::IsNullOrEmpty($registryName)) {
        $registryName=$serviceName
    }
    Write-Host "Creating Azure Container Registry named $registryName" -ForegroundColor Yellow
    az acr create -n $registryName -g $resourceGroupName -l $location  --admin-enabled true --sku Basic

    # Show ACR credentials
    Write-Host "ACR $registryName credentials:" -ForegroundColor Yellow
    az acr credential show -n $registryName
}