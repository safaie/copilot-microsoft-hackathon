# create dotnet 8 image for the current project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy the project file and restore the dependencies
COPY MinimalAPI.csproj .

# clean the project
RUN dotnet clean

# copy the source code and build the application
COPY . .
RUN dotnet restore # Ensure all dependencies are restored
RUN dotnet publish -c release -o /app --no-restore

# create a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# expose port 80
EXPOSE 80
ENTRYPOINT ["dotnet", "MinimalAPI.dll"]



