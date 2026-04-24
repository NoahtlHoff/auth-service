FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG AZURE_ARTIFACTS_PAT

WORKDIR /src

COPY nuget.config ./
RUN if [ -n "$AZURE_ARTIFACTS_PAT" ]; then \
      dotnet nuget update source auth-service \
        --username "docker" \
        --password "$AZURE_ARTIFACTS_PAT" \
        --store-password-in-clear-text \
        --configfile nuget.config; \
    fi

COPY ["AuthService.Api/AuthService.Api.csproj", "AuthService.Api/"]
COPY ["AuthService.Services/AuthService.Services.csproj", "AuthService.Services/"]
COPY ["AuthService.Repository/AuthService.Repository.csproj", "AuthService.Repository/"]
COPY ["AuthService.Interfaces/AuthService.Interfaces.csproj", "AuthService.Interfaces/"]
COPY ["AuthService.Models/AuthService.Models.csproj", "AuthService.Models/"]
COPY ["AuthService.Contracts/AuthService.Contracts.csproj", "AuthService.Contracts/"]

RUN dotnet restore "AuthService.Api/AuthService.Api.csproj"

COPY . .
WORKDIR /src/AuthService.Api
RUN dotnet build "AuthService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AuthService.Api.csproj" \
      -c $BUILD_CONFIGURATION \
      -o /app/publish \
      /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.Api.dll"]