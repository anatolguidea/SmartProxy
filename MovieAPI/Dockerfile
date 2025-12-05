FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY MovieAPI/MovieAPI.csproj MovieAPI/
COPY Common/Common.csproj Common/
RUN dotnet restore MovieAPI/MovieAPI.csproj

# Copy the remaining source
COPY MovieAPI/ MovieAPI/
COPY Common/ Common/

RUN dotnet publish MovieAPI/MovieAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MovieAPI.dll"]
