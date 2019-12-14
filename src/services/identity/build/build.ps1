﻿Param(
    [parameter(Mandatory=$true)][string]$subscription,
    [parameter(Mandatory=$true)][string]$resourceGroupName,
    [parameter(Mandatory=$false)][string]$acrName,
    [parameter(Mandatory=$false)][string]$gitUser="danielwoodhead",
    [parameter(Mandatory=$false)][string]$repoName="app",
    [parameter(Mandatory=$false)][string]$gitBranch="master",
    [parameter(Mandatory=$false)][string]$tag="v1"
)

$gitContext = "https://github.com/$gitUser/$repoName.git#$gitBranch"

$services = @(
    @{ Image="app/identity.api.$gitBranch`:$tag"; Context="src/services/identity"; File="src/Identity.API/Dockerfile" }
)

Write-Host "Setting subscription..." -ForegroundColor Yellow
az account set --subscription $subscription

$services |% {
    $image = $_.Image
    $context = $_.Context
    $file = $_.File
    Write-Host "Setting ACR build $image $context/$file"
    az acr build --registry $acrName $gitContext`:$context --image ${image} --file ${file} --resource-group $resourceGroupName
}
