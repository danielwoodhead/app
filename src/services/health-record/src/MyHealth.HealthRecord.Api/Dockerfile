#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["services/health-record/src/MyHealth.HealthRecord.Api/MyHealth.HealthRecord.Api.csproj", "services/health-record/src/MyHealth.HealthRecord.Api/"]
COPY ["services/health-record/src/MyHealth.HealthRecord.Core/MyHealth.HealthRecord.Core.csproj", "services/health-record/src/MyHealth.HealthRecord.Core/"]
COPY ["services/health-record/src/MyHealth.HealthRecord.Data.Fhir/MyHealth.HealthRecord.Data.Fhir.csproj", "services/health-record/src/MyHealth.HealthRecord.Data.Fhir/"]
COPY ["services/health-record/src/MyHealth.HealthRecord.Models/MyHealth.HealthRecord.Models.csproj", "services/health-record/src/MyHealth.HealthRecord.Models/"]
COPY ["extensions/src/MyHealth.Extensions.AspNetCore.Context/MyHealth.Extensions.AspNetCore.Context.csproj", "extensions/src/MyHealth.Extensions.AspNetCore.Context/"]
COPY ["extensions/src/MyHealth.Extensions.AspNetCore.Swagger/MyHealth.Extensions.AspNetCore.Swagger.csproj", "extensions/src/MyHealth.Extensions.AspNetCore.Swagger/"]
COPY ["extensions/src/MyHealth.Extensions.AspNetCore.Versioning/MyHealth.Extensions.AspNetCore.Versioning.csproj", "extensions/src/MyHealth.Extensions.AspNetCore.Versioning/"]
COPY ["extensions/src/MyHealth.Extensions.DependencyInjection/MyHealth.Extensions.DependencyInjection.csproj", "extensions/src/MyHealth.Extensions.DependencyInjection/"]
COPY ["extensions/src/MyHealth.Extensions.Events/MyHealth.Extensions.Events.csproj", "extensions/src/MyHealth.Extensions.Events/"]
COPY ["extensions/src/MyHealth.Extensions.Events.ApplicationInsights/MyHealth.Extensions.Events.ApplicationInsights.csproj", "extensions/src/MyHealth.Extensions.Events.ApplicationInsights/"]
COPY ["extensions/src/MyHealth.Extensions.Events.Azure.EventGrid/MyHealth.Extensions.Events.Azure.EventGrid.csproj", "extensions/src/MyHealth.Extensions.Events.Azure.EventGrid/"]
COPY ["extensions/src/MyHealth.Extensions.Fhir/MyHealth.Extensions.Fhir.csproj", "extensions/src/MyHealth.Extensions.Fhir/"]

RUN dotnet restore "services/health-record/src/MyHealth.HealthRecord.Api/MyHealth.HealthRecord.Api.csproj"
COPY . .
WORKDIR "/src/services/health-record/src/MyHealth.HealthRecord.Api"
RUN dotnet build "MyHealth.HealthRecord.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyHealth.HealthRecord.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyHealth.HealthRecord.Api.dll"]
