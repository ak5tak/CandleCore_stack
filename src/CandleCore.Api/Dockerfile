# Build context: repository root
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/CandleCore.Api/CandleCore.Api.csproj src/CandleCore.Api/
RUN dotnet restore src/CandleCore.Api/CandleCore.Api.csproj

COPY src/CandleCore.Api/ src/CandleCore.Api/
RUN dotnet publish src/CandleCore.Api/CandleCore.Api.csproj -c Release -o /app/publish --no-restore

# Framework-dependent migration bundle (runs on aspnet runtime image)
RUN dotnet tool install --global dotnet-ef --version 9.0.*
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet ef migrations bundle \
    --project src/CandleCore.Api/CandleCore.Api.csproj \
    --context AppDbContext \
    --output /migrate/efbundle \
    --force

# One-shot migrate image (no app publish payload required)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS migrate
WORKDIR /migrate
COPY --from=build /migrate/efbundle .
RUN chmod +x /migrate/efbundle
ENTRYPOINT ["/migrate/efbundle"]

# API runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CandleCore.Api.dll"]
