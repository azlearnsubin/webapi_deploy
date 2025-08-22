FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApp_DeployTest.csproj", "./"]
RUN dotnet restore
COPY . .
# /app/build creates absolute path - not inside src
# app/build  - relative to src - so inside src
RUN dotnet build "WebApp_DeployTest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WebApp_DeployTest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
# WORKDIR /app <- WORKDIR inherited from base
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApp_DeployTest.dll"]
