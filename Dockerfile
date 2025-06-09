# Stage 1: Build the ASP.NET 9.0 application
# Uses the official .NET 9.0 SDK image from Microsoft Container Registry.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the .csproj file(s) first. This allows Docker to cache this layer,
# so subsequent builds are faster if only source code changes.
# Assuming your project file is at BACKEND/BACKEND.csproj relative to your repo root.
COPY ["BACKEND/BACKEND.csproj", "BACKEND/"]

# Restore NuGet packages based on the copied .csproj.
RUN dotnet restore "BACKEND/BACKEND.csproj"

# Copy the rest of your application source code.
# This copies everything from the build context (your Git repository).
COPY . .

# Change to the project directory where your .csproj is located.
# This ensures dotnet publish runs in the correct context.
WORKDIR "/src/BACKEND"

# Publish the application for release. The output will be placed in the '/app/publish' directory.
# `--no-restore` is added as restore was already done in a previous step, optimizing the build.
RUN dotnet publish "BACKEND.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Create the final runtime image for deployment
# Uses the smaller ASP.NET 9.0 runtime image. This minimizes the final image size,
# as it only includes what's necessary to run the application, not to build it.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copy the published application files from the 'build' stage to the 'final' image.
COPY --from=build /app/publish .

# Expose port 80, which is the default port ASP.NET Core apps listen on inside the container.
# This is crucial for documentation and helps hosting platforms route traffic correctly.
EXPOSE 80

# Define the entry point for the container. This command runs your application when the container starts.
# Ensure 'BACKEND.dll' is the actual name of your compiled application's main DLL.
ENTRYPOINT ["dotnet", "BACKEND.dll"]
