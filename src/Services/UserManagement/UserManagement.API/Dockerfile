FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
# COPY ["nuget.config", "."]
COPY ["src/Services/UserManagement/UserManagement.API/UserManagement.API.csproj", "src/Services/UserManagement/UserManagement.API/"]
COPY ["src/Services/UserManagement/UserManagement.Core/UserManagement.Core.csproj", "src/Services/UserManagement/UserManagement.Core/"]
COPY ["src/Services/UserManagement/UserManagement.Infrastructure/UserManagement.Infrastructure.csproj", "src/Services/UserManagement/UserManagement.Infrastructure/"]
COPY ["src/Services/UserManagement/UserManagement.Domain/UserManagement.Domain.csproj", "src/Services/UserManagement/UserManagement.Domain/"]
COPY ["src/Services/Shared.Core/Shared.Core.csproj", "src/Services/Shared.Core/"]
RUN dotnet restore "src/Services/UserManagement/UserManagement.API/UserManagement.API.csproj"
COPY . .
WORKDIR "/src/src/Services/UserManagement/UserManagement.API"
RUN dotnet build "UserManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM build AS test
WORKDIR /source/test

COPY test/InnoShop.UnitTests/*.csproj .5
RUN dotnet restore tests.csproj

COPY tests/ .
RUN dotnet build --no-restore

ENTRYPOINT ["dotnet", "test", "--logger:trx", "--no-restore", "--no-build"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManagement.API.dll"]