FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files to restore dependencies
COPY SmartProxy.sln ./
COPY Common/Common.csproj Common/
COPY MovieAPI/MovieAPI.csproj MovieAPI/
COPY Proxy/Proxy.csproj Proxy/
COPY SyncNode/SyncNode.csproj SyncNode/

RUN dotnet restore Proxy/Proxy.csproj

# Copy the rest of the source and publish the Proxy service
COPY . .
RUN dotnet publish Proxy/Proxy.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Proxy.dll"]
