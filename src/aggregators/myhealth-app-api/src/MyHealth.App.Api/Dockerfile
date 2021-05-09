#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["aggregators/myhealth-app-api/src/MyHealth.App.Api/MyHealth.App.Api.csproj", "aggregators/myhealth-app-api/src/MyHealth.App.Api/"]
COPY ["aggregators/myhealth-app-api/src/apis/MyHealth.App.Api.HealthRecord/MyHealth.App.Api.HealthRecord.csproj", "aggregators/myhealth-app-api/src/apis/MyHealth.App.Api.HealthRecord/"]
COPY ["aggregators/myhealth-app-api/src/apis/MyHealth.App.Api.Identity/MyHealth.App.Api.Identity.csproj", "aggregators/myhealth-app-api/src/apis/MyHealth.App.Api.Identity/"]
COPY ["aggregators/myhealth-app-api/src/apis/MyHealth.App.Api.Integrations/MyHealth.App.Api.Integrations.csproj", "aggregators/myhealth-app-api/src/apis/MyHealth.App.Api.Integrations/"]
COPY ["aggregators/myhealth-app-api/src/MyHealth.App.Api.Core/MyHealth.App.Api.Core.csproj", "aggregators/myhealth-app-api/src/MyHealth.App.Api.Core/"]
COPY ["extensions/src/MyHealth.Extensions.AspNetCore.Swagger/MyHealth.Extensions.AspNetCore.Swagger.csproj", "extensions/src/MyHealth.Extensions.AspNetCore.Swagger/"]
COPY ["extensions/src/MyHealth.Extensions.AspNetCore.Versioning/MyHealth.Extensions.AspNetCore.Versioning.csproj", "extensions/src/MyHealth.Extensions.AspNetCore.Versioning/"]
RUN dotnet restore "aggregators/myhealth-app-api/src/MyHealth.App.Api/MyHealth.App.Api.csproj"
COPY . .
WORKDIR "/src/aggregators/myhealth-app-api/src/MyHealth.App.Api"
RUN dotnet build "MyHealth.App.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyHealth.App.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyHealth.App.Api.dll"]
