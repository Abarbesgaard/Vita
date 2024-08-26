# Byg applikationen
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopier projektfiler og restore afhængigheder
COPY Vita_WebApi_API/Vita_WebApi_API.csproj Vita_WebApi_API/
COPY Vita_WebAPI_Services/Vita_WebAPI_Services.csproj Vita_WebAPI_Services/
COPY Vita_WebAPI_Repository/Vita_WebAPI_Repository.csproj Vita_WebAPI_Repository/
COPY Vita_WebAPI_Data/Vita_WebAPI_Data.csproj Vita_WebAPI_Data/
COPY Vita_WebApi_Shared/Vita_WebApi_Shared.csproj Vita_WebApi_Shared/
RUN dotnet restore Vita_WebApi_API/Vita_WebApi_API.csproj

# Debug-kommando til at liste filer
RUN ls -l Vita_WebApi_API/
RUN ls -l Vita_WebAPI_Services/
RUN ls -l Vita_WebAPI_Repository/
RUN ls -l Vita_WebAPI_Data/
RUN ls -l Vita_WebApi_Shared/
# Kopier resten af koden og byg
COPY . .
RUN dotnet build Vita_WebApi_API/Vita_WebApi_API.csproj -c Release -o /app/build

# Opret runtime-billede
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Vita_WebApi_API.dll"]
