FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY SignalRPoc.Server.sln SignalRPoc.Server.sln
COPY SignalRPoc.Server.Application/SignalRPoc.Server.Application.csproj SignalRPoc.Server.Application/SignalRPoc.Server.Application.csproj
COPY SignalRPoc.Server.Domain/SignalRPoc.Server.Domain.csproj SignalRPoc.Server.Domain/SignalRPoc.Server.Domain.csproj
COPY SignalRPoc.Server.Infrastructure/SignalRPoc.Server.Infrastructure.csproj SignalRPoc.Server.Infrastructure/SignalRPoc.Server.Infrastructure.csproj
COPY SignalRPoc.Server.WebApi/SignalRPoc.Server.WebApi.csproj SignalRPoc.Server.WebApi/SignalRPoc.Server.WebApi.csproj
RUN dotnet restore

COPY . .

RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

RUN apt-get update && apt-get install -y curl --no-install-recommends && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

RUN groupadd -g 10000 dotnet && useradd -u 10000 -g dotnet dotnet && chown -R dotnet:dotnet /app
USER dotnet:dotnet

ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl --fail http://localhost:5000/health || exit 1

ENTRYPOINT ["dotnet", "SignalRPoc.Server.WebApi.dll"]
