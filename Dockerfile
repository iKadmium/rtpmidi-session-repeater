# Use the official .NET SDK image as the base image
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project file(s) to the container
COPY ./*.csproj ./

# Restore the NuGet packages
RUN dotnet restore

# Copy the remaining source code to the container
COPY . ./

# Build the application
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish -c Release --no-restore --no-build -o out

# Use the official .NET runtime image as the base image for the final image
FROM mcr.microsoft.com/dotnet/runtime:8.0

# Set the working directory inside the container
WORKDIR /app

# Copy the published output from the build image to the final image
COPY --from=build /app/out ./

# Set the entry point for the container
ENTRYPOINT ["dotnet", "rtpmidi-session-repeater.dll"]