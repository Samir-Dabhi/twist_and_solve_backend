# Use the official .NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Switch to a non-root user (use existing 'app' user if available)
USER app

# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the .csproj file separately for better caching
COPY twist_and_solve_backend.csproj .
RUN dotnet restore "twist_and_solve_backend.csproj"

# Copy the rest of the application and build
COPY . .
RUN dotnet build "twist_and_solve_backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "twist_and_solve_backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the runtime image for final deployment
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set ownership to 'app' user (in case of permission issues)
USER app

# Run the application
ENTRYPOINT ["dotnet", "twist_and_solve_backend.dll"]
