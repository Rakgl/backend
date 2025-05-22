# Use .NET 9.0 SDK for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Use .NET 9.0 runtime for execution
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Create uploads directory for profile images and set permissions
RUN mkdir -p /app/wwwroot/uploads/profiles && chmod -R 755 /app/wwwroot/uploads/profiles

# Expose port (default ASP.NET Core port)
EXPOSE 80

# Set environment variable for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:80

# Entry point for the application
ENTRYPOINT ["./BACKEND"]