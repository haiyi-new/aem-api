# Use the official .NET SDK image for build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS dev
WORKDIR /app

# Copy both project and code files to leverage .csproj for package installation
COPY . .

# Restore packages directly from .csproj
RUN dotnet restore

# Install migration tool globally
RUN dotnet tool install --global dotnet-ef --version 7.0.5

# Add global tools directory to PATH
# ENV PATH="${PATH}:/root/.dotnet/tools"

# Change local host, ease other to run docker compose without port problem
ENV DOTNET_URLS=http://+:5218

EXPOSE 5218

# Entry point for development with hot reload
ENTRYPOINT ["dotnet", "watch", "run"]

FROM dev AS build
WORKDIR /app

# Build the application
RUN dotnet publish -c Release -o out

