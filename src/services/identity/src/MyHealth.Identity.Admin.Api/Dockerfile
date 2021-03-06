FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/MyHealth.Identity.Admin.Api/MyHealth.Identity.Admin.Api.csproj", "src/MyHealth.Identity.Admin.Api/"]
COPY ["src/MyHealth.Identity.Admin.EntityFramework.Shared/MyHealth.Identity.Admin.EntityFramework.Shared.csproj", "src/MyHealth.Identity.Admin.EntityFramework.Shared/"]
COPY ["src/MyHealth.Identity.Admin.EntityFramework.SqlServer/MyHealth.Identity.Admin.EntityFramework.SqlServer.csproj", "src/MyHealth.Identity.Admin.EntityFramework.SqlServer/"]
COPY ["src/MyHealth.Identity.Admin.EntityFramework.PostgreSQL/MyHealth.Identity.Admin.EntityFramework.PostgreSQL.csproj", "src/MyHealth.Identity.Admin.EntityFramework.PostgreSQL/"]
COPY ["src/MyHealth.Identity.Shared/MyHealth.Identity.Shared.csproj", "src/MyHealth.Identity.Shared/"]
COPY ["src/MyHealth.Identity.Admin.EntityFramework.MySql/MyHealth.Identity.Admin.EntityFramework.MySql.csproj", "src/MyHealth.Identity.Admin.EntityFramework.MySql/"]
RUN dotnet restore "src/MyHealth.Identity.Admin.Api/MyHealth.Identity.Admin.Api.csproj"
COPY . .
WORKDIR "/src/src/MyHealth.Identity.Admin.Api"
RUN dotnet build "MyHealth.Identity.Admin.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyHealth.Identity.Admin.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENTRYPOINT ["dotnet", "MyHealth.Identity.Admin.Api.dll"]