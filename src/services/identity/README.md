# Identity Service

Identity Service using IdentityServer4 and .NET 3.1

## Build

The build process (see build/build.ps1) uses `az acr build` to build and push the docker image to Azure Container Registry.

## Deploy

The deployment process (see deploy/k8s/deploy.ps1) uses helm to deploy the service to kubernetes. The helm chart should first be pushed to Azure Container Registry (see deploy/k8s/helm/push.ps1).

### TODO

1. Distinguish between `helm install` and `helm upgrade`
2. Pass the tag in instead of it being hardcoded
3. Look into --name-template string (specify template used to name the release)
4. Investigate having a set of 'core' charts used across services