# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["MultiLanguageDemo/MultiLanguageDemo.csproj", "MultiLanguageDemo/"]
RUN dotnet restore "MultiLanguageDemo/MultiLanguageDemo.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/MultiLanguageDemo"
RUN dotnet build "MultiLanguageDemo.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MultiLanguageDemo.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 8 runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy the published application
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Create a non-root user and switch to it
RUN useradd -m -u 1000 appuser && chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "MultiLanguageDemo.dll"]
