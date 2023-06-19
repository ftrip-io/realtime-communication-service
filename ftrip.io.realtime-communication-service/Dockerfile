FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /build

ARG CONFIGURATION=Release

# Restore dependencies
COPY ["ftrip.io.rtc-service.contracts/*.csproj", "ftrip.io.rtc-service.contracts/package*.json", "./ftrip.io.rtc-service.contracts/"]
RUN dotnet restore /p:Configuration=$CONFIGURATION ftrip.io.rtc-service.contracts/ftrip.io.rtc-service.contracts.csproj

COPY ["ftrip.io.rtc-service/*.csproj", "ftrip.io.rtc-service/package*.json", "./ftrip.io.rtc-service/"]
RUN dotnet restore /p:Configuration=$CONFIGURATION ftrip.io.rtc-service/ftrip.io.rtc-service.csproj

# Publish
COPY . .

RUN dotnet publish -c $CONFIGURATION -o out ftrip.io.rtc-service/ftrip.io.rtc-service.csproj
 
# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS final
WORKDIR /app

RUN addgroup --system ftrip-io
RUN adduser --system rtc-service

COPY --from=build --chown=rtc-service:ftrip-io --chmod=500 /build/out .

EXPOSE 80

USER rtc-service

ENTRYPOINT ["dotnet", "ftrip.io.rtc-service.dll"]