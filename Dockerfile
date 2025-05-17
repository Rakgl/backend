# Use the official ASP.NET 9.0 SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["BACKEND/BACKEND.csproj", "BACKEND/"]
RUN dotnet restore "BACKEND/BACKEND.csproj"

# Copy the rest of the project files
COPY . .
WORKDIR "/src/BACKEND"

# Build the application
RUN dotnet build "BACKEND.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "BACKEND.csproj" -c Release -o /app/publish

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Expose port 80
EXPOSE 80

ENTRYPOINT ["dotnet", "BACKEND.dll"]