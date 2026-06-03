FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY src/LuyPOS.Api/LuyPOS.Api.csproj src/LuyPOS.Api/
RUN dotnet restore src/LuyPOS.Api/LuyPOS.Api.csproj

COPY . .
RUN dotnet publish src/LuyPOS.Api/LuyPOS.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LuyPOS.Api.dll"]
