# Use the official .NET SDK image for build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source code
COPY . ./

# Install additional NuGet packages
RUN dotnet add package System.Net.Http
RUN dotnet add package Newtonsoft.Json
RUN dotnet add package MySql.Data

# Build the application
RUN dotnet publish -c Release -o out

# Build runtime image with MySQL connector
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
COPY --from=build /app/out/*.dll /app/
COPY --from=build /app/out/*.deps.json /app/
COPY --from=build /app/out/*.runtimeconfig.json /app/

# Expose port for web application
EXPOSE 80

# Define the entry point for the application, connecting to MySQL
ENTRYPOINT ["dotnet", "MyWebApi.dll"]
