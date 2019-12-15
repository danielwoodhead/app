# must be run from 'identity' directory

az account set --subscription bc22730d-89ef-4562-9a78-dfb790976b9a
az acr build --registry DansContainerRegistry --image "app/identity.api.local`:{{.Run.ID}}" --file src/Identity.API/Dockerfile .