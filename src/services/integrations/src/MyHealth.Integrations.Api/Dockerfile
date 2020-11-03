#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["services/integrations/src/MyHealth.Integrations.Api/MyHealth.Integrations.Api.csproj", "services/integrations/src/MyHealth.Integrations.Api/"]
COPY ["services/integrations/src/MyHealth.Integrations.Core/MyHealth.Integrations.Core.csproj", "services/integrations/src/MyHealth.Integrations.Core/"]
COPY ["services/integrations/src/MyHealth.Integrations.Data.Cosmos/MyHealth.Integrations.Data.Cosmos.csproj", "services/integrations/src/MyHealth.Integrations.Data.Cosmos/"]
COPY ["services/integrations/src/MyHealth.Integrations.IoMT.EventHub/MyHealth.Integrations.IoMT.EventHub.csproj", "services/integrations/src/MyHealth.Integrations.IoMT.EventHub/"]
COPY ["services/integrations/src/MyHealth.Integrations.Models/MyHealth.Integrations.Models.csproj", "services/integrations/src/MyHealth.Integrations.Models/"]
COPY ["services/integrations/src/MyHealth.Integrations.Utility/MyHealth.Integrations.Utility.csproj", "services/integrations/src/MyHealth.Integrations.Utility/"]
COPY ["services/integrations/src/integrations/MyHealth.Integrations.Fitbit/MyHealth.Integrations.Fitbit.csproj", "services/integrations/src/integrations/MyHealth.Integrations.Fitbit/"]
COPY ["services/integrations/src/integrations/MyHealth.Integrations.Strava/MyHealth.Integrations.Strava.csproj", "services/integrations/src/integrations/MyHealth.Integrations.Strava/"]
COPY ["extensions/src/MyHealth.Extensions.AspNetCore.Context/MyHealth.Extensions.AspNetCore.Context.csproj", "extensions/src/MyHealth.Extensions.AspNetCore.Context/"]
COPY ["extensions/src/MyHealth.Extensions.AspNetCore.Swagger/MyHealth.Extensions.AspNetCore.Swagger.csproj", "extensions/src/MyHealth.Extensions.AspNetCore.Swagger/"]
COPY ["extensions/src/MyHealth.Extensions.AspNetCore.Versioning/MyHealth.Extensions.AspNetCore.Versioning.csproj", "extensions/src/MyHealth.Extensions.AspNetCore.Versioning/"]
COPY ["extensions/src/MyHealth.Extensions.Cryptography/MyHealth.Extensions.Cryptography.csproj", "extensions/src/MyHealth.Extensions.Cryptography/"]
COPY ["extensions/src/MyHealth.Extensions.DependencyInjection/MyHealth.Extensions.DependencyInjection.csproj", "extensions/src/MyHealth.Extensions.DependencyInjection/"]
COPY ["extensions/src/MyHealth.Extensions.Events/MyHealth.Extensions.Events.csproj", "extensions/src/MyHealth.Extensions.Events/"]
COPY ["extensions/src/MyHealth.Extensions.Events.ApplicationInsights/MyHealth.Extensions.Events.ApplicationInsights.csproj", "extensions/src/MyHealth.Extensions.Events.ApplicationInsights/"]
COPY ["extensions/src/MyHealth.Extensions.Events.Azure.EventGrid/MyHealth.Extensions.Events.Azure.EventGrid.csproj", "extensions/src/MyHealth.Extensions.Events.Azure.EventGrid/"]
COPY ["extensions/src/MyHealth.Extensions.Fhir/MyHealth.Extensions.Fhir.csproj", "extensions/src/MyHealth.Extensions.Fhir/"]
RUN dotnet restore "services/integrations/src/MyHealth.Integrations.Api/MyHealth.Integrations.Api.csproj"
COPY . .
WORKDIR "/src/services/integrations/src/MyHealth.Integrations.Api"
RUN dotnet build "MyHealth.Integrations.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyHealth.Integrations.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyHealth.Integrations.Api.dll"]
