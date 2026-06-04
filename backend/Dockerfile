FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY src/LuyPOS.Api/LuyPOS.Api.csproj src/LuyPOS.Api/
RUN dotnet restore src/LuyPOS.Api/LuyPOS.Api.csproj

COPY . .
RUN dotnet publish src/LuyPOS.Api/LuyPOS.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dev

WORKDIR /src
COPY src/LuyPOS.Api/LuyPOS.Api.csproj src/LuyPOS.Api/
RUN dotnet restore src/LuyPOS.Api/LuyPOS.Api.csproj

COPY . .
ENV DOTNET_USE_POLLING_FILE_WATCHER=1 \
    DOTNET_WATCH_RESTART_ON_RUDE_EDIT=1 \
    DOTNET_CLI_TELEMETRY_OPTOUT=1
ENTRYPOINT ["dotnet", "watch", "--no-hot-reload", "--project", "src/LuyPOS.Api/LuyPOS.Api.csproj", "run", "--urls", "http://0.0.0.0:8080"]

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LuyPOS.Api.dll"]
