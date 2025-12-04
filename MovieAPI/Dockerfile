FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY SmartProxy.sln ./
COPY Common/Common.csproj Common/
COPY MovieAPI/MovieAPI.csproj MovieAPI/
COPY Proxy/Proxy.csproj Proxy/
COPY SyncNode/SyncNode.csproj SyncNode/

RUN dotnet restore MovieAPI/MovieAPI.csproj

COPY . .
RUN dotnet publish MovieAPI/MovieAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MovieAPI.dll"]
