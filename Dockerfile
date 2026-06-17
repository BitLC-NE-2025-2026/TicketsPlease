FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/TicketsPlease.Web/TicketsPlease.Web.csproj", "src/TicketsPlease.Web/"]
COPY ["src/TicketsPlease.Infrastructure/TicketsPlease.Infrastructure.csproj", "src/TicketsPlease.Infrastructure/"]
COPY ["src/TicketsPlease.Application/TicketsPlease.Application.csproj", "src/TicketsPlease.Application/"]
COPY ["src/TicketsPlease.Domain/TicketsPlease.Domain.csproj", "src/TicketsPlease.Domain/"]
RUN dotnet restore "src/TicketsPlease.Web/TicketsPlease.Web.csproj"
COPY . .
WORKDIR "/src/src/TicketsPlease.Web"
RUN dotnet build "TicketsPlease.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicketsPlease.Web.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketsPlease.Web.dll"]
