## Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and restore as distinct layers for better caching
COPY SnapToTable.sln ./
COPY src/SnapToTable.API/SnapToTable.API.csproj src/SnapToTable.API/
COPY src/SnapToTable.Application/SnapToTable.Application.csproj src/SnapToTable.Application/
COPY src/SnapToTable.Domain/SnapToTable.Domain.csproj src/SnapToTable.Domain/
COPY src/SnapToTable.Infrastructure/SnapToTable.Infrastructure.csproj src/SnapToTable.Infrastructure/

RUN dotnet restore src/SnapToTable.API/SnapToTable.API.csproj

# Copy the rest of the source
COPY src/ ./src/

# Build
RUN dotnet publish src/SnapToTable.API/SnapToTable.API.csproj -c Release -o /app/publish /p:UseAppHost=false


## Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy build output
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "SnapToTable.API.dll"]


