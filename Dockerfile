# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the solution file and restore dependencies
COPY *.sln .
COPY DatabaseReader/DatabaseReader/DatabaseReader.csproj ./DatabaseReader/
RUN dotnet restore ./DatabaseReader/DatabaseReader.csproj

# Copy the entire project and build
COPY . .
RUN dotnet build -c Release --no-restore ./DatabaseReader/DatabaseReader.csproj

# Stage 2: Publish the application
FROM build AS publish
RUN dotnet publish ./DatabaseReader/DatabaseReader.csproj -c Release --no-build -o /app/publish

# Stage 3: Create the final runtime image
FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app

# Copy the published output from the previous stage
COPY --from=publish /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "DatabaseReader.dll"]
