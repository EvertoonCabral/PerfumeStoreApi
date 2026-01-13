# =========================
# BASE / RUNTIME
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# =========================
# BUILD
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["PerfumeStoreApi/PerfumeStoreApi.csproj", "PerfumeStoreApi/"]
RUN dotnet restore "PerfumeStoreApi/PerfumeStoreApi.csproj"

COPY . .
WORKDIR "/src/PerfumeStoreApi"
RUN dotnet build "PerfumeStoreApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# =========================
# PUBLISH
# =========================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PerfumeStoreApi.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# FINAL
# =========================
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PerfumeStoreApi.dll"]
