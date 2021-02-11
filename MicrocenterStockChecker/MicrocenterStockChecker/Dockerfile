FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["MicrocenterStockChecker/MicrocenterStockChecker.csproj", "MicrocenterStockChecker/"]
RUN dotnet restore "MicrocenterStockChecker/MicrocenterStockChecker.csproj"
COPY . .
WORKDIR "/src/MicrocenterStockChecker"
RUN dotnet build "MicrocenterStockChecker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicrocenterStockChecker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicrocenterStockChecker.dll"]