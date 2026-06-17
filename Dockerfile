# Basisabbild fuer den Build Prozess mit .NET 10.0 SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Kopieren der Projektdateien fuer den Restore Vorgang
COPY ["src/TicketsPlease.Web/TicketsPlease.Web.csproj", "src/TicketsPlease.Web/"]
COPY ["src/TicketsPlease.Infrastructure/TicketsPlease.Infrastructure.csproj", "src/TicketsPlease.Infrastructure/"]
COPY ["src/TicketsPlease.Application/TicketsPlease.Application.csproj", "src/TicketsPlease.Application/"]
COPY ["src/TicketsPlease.Domain/TicketsPlease.Domain.csproj", "src/TicketsPlease.Domain/"]

# Wiederherstellen der NuGet Pakete
RUN dotnet restore "src/TicketsPlease.Web/TicketsPlease.Web.csproj"

# Kopieren des restlichen Quellcodes
COPY . .
WORKDIR "/src/src/TicketsPlease.Web"

# Kompilieren der Anwendung im Release Modus
RUN dotnet build "TicketsPlease.Web.csproj" -c Release -o /app/build

# Veroeffentlichen der Anwendung
FROM build AS publish
RUN dotnet publish "TicketsPlease.Web.csproj" -c Release -o /app/publish

# Laufzeitumgebung mit .NET 10.0 ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

# Kopieren der veroeffentlichten Dateien aus dem Publish Layer
COPY --from=publish /app/publish .

# Startpunkt der Anwendung definieren
ENTRYPOINT ["dotnet", "TicketsPlease.Web.dll"]
