FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY SyncNode/SyncNode.csproj SyncNode/
COPY Common/Common.csproj Common/
RUN dotnet restore SyncNode/SyncNode.csproj

# Copy the remaining source
COPY SyncNode/ SyncNode/
COPY Common/ Common/

RUN dotnet publish SyncNode/SyncNode.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SyncNode.dll"]
