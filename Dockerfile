# Use a .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /BACKEND
COPY ["BACKEND/BACKEND.csproj", "BACKEND/"]
RUN dotnet restore "BACKEND/BACKEND.csproj"
COPY . .
WORKDIR "/BACKEND/BACKEND"
RUN dotnet publish "BACKEND.csproj" -c Release -o /app/publish

# Use a .NET 9.0 runtime image for the final application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BACKEND.dll"]