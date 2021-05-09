#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["services/events/src/MyHealth.Events.Api/MyHealth.Events.Api.csproj", "services/events/src/MyHealth.Events.Api/"]
COPY ["services/events/src/MyHealth.Events.EventIngestion/MyHealth.Events.EventIngestion.csproj", "services/events/src/MyHealth.Events.EventIngestion/"]
COPY ["services/events/src/MyHealth.Events.Azure.EventGrid/MyHealth.Events.Azure.EventGrid.csproj", "services/events/src/MyHealth.Events.Azure.EventGrid/"]
COPY ["services/events/src/MyHealth.Events.Azure.TableStorage/MyHealth.Events.Azure.TableStorage.csproj", "services/events/src/MyHealth.Events.Azure.TableStorage/"]
COPY ["extensions/src/MyHealth.Extensions.AspNetCore.Swagger/MyHealth.Extensions.AspNetCore.Swagger.csproj", "extensions/src/MyHealth.Extensions.AspNetCore.Swagger/"]
COPY ["extensions/src/MyHealth.Extensions.AspNetCore.Versioning/MyHealth.Extensions.AspNetCore.Versioning.csproj", "extensions/src/MyHealth.Extensions.AspNetCore.Versioning/"]

RUN dotnet restore "services/events/src/MyHealth.Events.Api/MyHealth.Events.Api.csproj"
COPY . .
WORKDIR "/src/services/events/src/MyHealth.Events.Api"
RUN dotnet build "MyHealth.Events.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyHealth.Events.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyHealth.Events.Api.dll"]