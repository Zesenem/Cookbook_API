# Stage 1: Build with .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY CookbookAPI.sln ./
COPY CookbookAPI/CookbookAPI/*.csproj CookbookAPI/CookbookAPI/
COPY CookbookAPI/CookbookAPI.ConsoleTest/*.csproj CookbookAPI/CookbookAPI.ConsoleTest/
COPY CookbookAPI/CookbookAPI.Data/*.csproj CookbookAPI/CookbookAPI.Data/
COPY CookbookAPI/CookbookAPI.Domain/*.csproj CookbookAPI/CookbookAPI.Domain/
COPY CookbookAPI/CookbookAPI.Repositories/*.csproj CookbookAPI/CookbookAPI.Repositories/
COPY CookbookAPI/CookbookAPI.Services/*.csproj CookbookAPI/CookbookAPI.Services/

# Restore all projects
RUN dotnet restore CookbookAPI.sln

# Copy everything else and publish
COPY . .
RUN dotnet publish CookbookAPI.sln -c Release -o /app/publish

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CookbookAPI.dll"]
