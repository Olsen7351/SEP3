# Use the official .NET SDK image to build the Class Library
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory in the container
WORKDIR /src

# Copy the .csproj and restore as distinct layers
COPY ClassLibrary_SEP3/ClassLibrary_SEP3.csproj ClassLibrary_SEP3/
RUN dotnet restore ClassLibrary_SEP3/ClassLibrary_SEP3.csproj

# Copy the remaining source code
COPY ClassLibrary_SEP3/ ClassLibrary_SEP3/

# Build the Class Library
RUN dotnet build ClassLibrary_SEP3/ClassLibrary_SEP3.csproj -c Release -o /app/build

# Publish the Class Library for production
FROM build AS publish
RUN dotnet publish ClassLibrary_SEP3/ClassLibrary_SEP3.csproj -c Release -o /app/publish

# Create a final image with only the Class Library
FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClassLibrary_SEP3.dll"]
