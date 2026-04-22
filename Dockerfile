# ---------- BUILD STAGE ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files (better caching)
COPY *.sln .
COPY src/ ./src/
COPY test/ ./test/

# Restore dependencies
RUN dotnet restore

# Build solution
RUN dotnet build -c Release --no-restore

# Run tests (FAILS if any test fails)
RUN dotnet test --no-build --verbosity normal

# Publish API
RUN dotnet publish src/WeatherApplication.API/WeatherApplication.API.csproj -c Release -o /app/out


# ---------- RUNTIME STAGE ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# ✅ Force .NET to run on port 80
ENV ASPNETCORE_URLS=http://+:80

# Optional but good practice
EXPOSE 80

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "WeatherApplication.API.dll"]