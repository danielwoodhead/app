FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY ["services/integrations/src/MyHealth.Integrations.FunctionApp/MyHealth.Integrations.FunctionApp.csproj", "services/integrations/src/MyHealth.Integrations.FunctionApp/"]
COPY ["services/integrations/src/MyHealth.Integrations.Core/MyHealth.Integrations.Core.csproj", "services/integrations/src/MyHealth.Integrations.Core/"]
COPY ["services/integrations/src/MyHealth.Integrations.IoMT.EventHub/MyHealth.Integrations.IoMT.EventHub.csproj", "services/integrations/src/MyHealth.Integrations.IoMT.EventHub/"]
COPY ["services/integrations/src/MyHealth.Integrations.Data.Cosmos/MyHealth.Integrations.Data.Cosmos.csproj", "services/integrations/src/MyHealth.Integrations.Data.Cosmos/"]
COPY ["services/integrations/src/MyHealth.Integrations.Utility/MyHealth.Integrations.Utility.csproj", "services/integrations/src/MyHealth.Integrations.Utility/"]
COPY ["services/integrations/src/integrations/MyHealth.Integrations.Fitbit/MyHealth.Integrations.Fitbit.csproj", "services/integrations/src/integrations/MyHealth.Integrations.Fitbit/"]
COPY ["extensions/src/MyHealth.Extensions.Events.Azure.EventGrid/MyHealth.Extensions.Events.Azure.EventGrid.csproj", "extensions/src/MyHealth.Extensions.Events.Azure.EventGrid/"]
COPY ["extensions/src/MyHealth.Extensions.Fhir/MyHealth.Extensions.Fhir.csproj", "extensions/src/MyHealth.Extensions.Fhir/"]
RUN dotnet restore "services/integrations/src/MyHealth.Integrations.FunctionApp/MyHealth.Integrations.FunctionApp.csproj"
COPY . .
WORKDIR "/src/services/integrations/src/MyHealth.Integrations.FunctionApp"
RUN dotnet build "MyHealth.Integrations.FunctionApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyHealth.Integrations.FunctionApp.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/azure-functions/dotnet:3.0
ENV AzureWebJobsScriptRoot=/app/publish \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=publish ["/app/publish", "/home/site/wwwroot"]
