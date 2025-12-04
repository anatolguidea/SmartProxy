FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY SmartProxy.sln ./
COPY Common/Common.csproj Common/
COPY SyncNode/SyncNode.csproj SyncNode/
COPY MovieAPI/MovieAPI.csproj MovieAPI/
COPY Proxy/Proxy.csproj Proxy/

RUN dotnet restore SyncNode/SyncNode.csproj

COPY . .
RUN dotnet publish SyncNode/SyncNode.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SyncNode.dll"]
