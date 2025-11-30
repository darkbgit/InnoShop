FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
# COPY ["nuget.config", "."]
COPY ["src/Services/ProductManagement/ProductManagement.API/ProductManagement.API.csproj", "src/Services/ProductManagement/ProductManagement.API/"]
COPY ["src/Services/ProductManagement/ProductManagement.Core/ProductManagement.Core.csproj", "src/Services/ProductManagement/ProductManagement.Core/"]
COPY ["src/Services/ProductManagement/ProductManagement.DataAccess/ProductManagement.DataAccess.csproj", "src/Services/ProductManagement/ProductManagement.DataAccess/"]
COPY ["src/Services/ProductManagement/ProductManagement.Domain/ProductManagement.Domain.csproj", "src/Services/ProductManagement/ProductManagement.Domain/"]
COPY ["src/Services/Shared.Core/Shared.Core.csproj", "src/Services/Shared.Core/"]
RUN dotnet restore "src/Services/ProductManagement/ProductManagement.API/ProductManagement.API.csproj"
COPY . .
WORKDIR "/src/src/Services/ProductManagement/ProductManagement.API"
RUN dotnet build "ProductManagement.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

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
ENTRYPOINT ["dotnet", "ProductManagement.API.dll"]