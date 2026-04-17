# ── Build stage ──
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

# Copy csproj first for layer caching
COPY src/APIMASTER/APIMASTER.csproj src/APIMASTER/
RUN dotnet restore src/APIMASTER/APIMASTER.csproj

# Copy everything and publish
COPY src/ src/
RUN dotnet publish src/APIMASTER/APIMASTER.csproj \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:DebugType=None \
    /p:DebugSymbols=false

# ── Runtime stage ──
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS runtime
WORKDIR /app

# Install curl for healthcheck
RUN apk add --no-cache curl

# Security: run as non-root user
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

# Create directories for file storage
RUN mkdir -p /app/static/cross_manifest /app/static/scalei /app/static/approvals \
    && chown -R appuser:appgroup /app/static

USER appuser

COPY --from=build --chown=appuser:appgroup /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_EnableDiagnostics=0

HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "APIMASTER.dll"]
