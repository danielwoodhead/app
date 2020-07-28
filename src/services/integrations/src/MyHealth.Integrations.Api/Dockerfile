#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["services/integrations/src/MyHealth.Integrations.Api/MyHealth.Integrations.Api.csproj", "services/integrations/src/MyHealth.Integrations.Api/"]
COPY ["services/integrations/src/MyHealth.Integrations.Repository.TableStorage/MyHealth.Integrations.Repository.TableStorage.csproj", "services/integrations/src/MyHealth.Integrations.Repository.TableStorage/"]
COPY ["extensions/src/MyHealth.Extensions.Azure.Storage.Table/MyHealth.Extensions.Azure.Storage.Table.csproj", "extensions/src/MyHealth.Extensions.Azure.Storage.Table/"]
COPY ["services/integrations/src/MyHealth.Integrations.Core/MyHealth.Integrations.Core.csproj", "services/integrations/src/MyHealth.Integrations.Core/"]
COPY ["services/integrations/src/MyHealth.Integrations.Models/MyHealth.Integrations.Models.csproj", "services/integrations/src/MyHealth.Integrations.Models/"]
COPY ["extensions/src/MyHealth.Extensions.Events/MyHealth.Extensions.Events.csproj", "extensions/src/MyHealth.Extensions.Events/"]
COPY ["services/integrations/src/MyHealth.Integrations.Utility/MyHealth.Integrations.Utility.csproj", "services/integrations/src/MyHealth.Integrations.Utility/"]
COPY ["extensions/src/MyHealth.Extensions.AspNetCore.Swagger/MyHealth.Extensions.AspNetCore.Swagger.csproj", "extensions/src/MyHealth.Extensions.AspNetCore.Swagger/"]
COPY ["services/integrations/src/providers/MyHealth.Integrations.Fitbit/MyHealth.Integrations.Fitbit.csproj", "services/integrations/src/providers/MyHealth.Integrations.Fitbit/"]
COPY ["extensions/src/MyHealth.Extensions.AspNetCore.Versioning/MyHealth.Extensions.AspNetCore.Versioning.csproj", "extensions/src/MyHealth.Extensions.AspNetCore.Versioning/"]
COPY ["extensions/src/MyHealth.Extensions.Events.Azure.EventGrid/MyHealth.Extensions.Events.Azure.EventGrid.csproj", "extensions/src/MyHealth.Extensions.Events.Azure.EventGrid/"]
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
