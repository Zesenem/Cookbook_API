# Stage 1: Build with .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything (solution + all projects)
COPY . .

# Restore and publish in one go
RUN dotnet restore CookbookAPI.sln
RUN dotnet publish CookbookAPI.sln -c Release -o /app/publish

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published artifacts
COPY --from=build /app/publish .

# Launch your API
ENTRYPOINT ["dotnet", "CookbookAPI.dll"]