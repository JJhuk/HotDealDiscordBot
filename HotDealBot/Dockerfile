FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HotDealBot/HotDealBot.csproj", "HotDealBot/"]
RUN dotnet restore "HotDealBot/HotDealBot.csproj"
COPY . .
WORKDIR "/src/HotDealBot"
RUN dotnet build "HotDealBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotDealBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotDealBot.dll"]
