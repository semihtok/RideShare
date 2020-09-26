FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RideShare.API/RideShare.API.csproj", "RideShare.API/"]
RUN dotnet restore "RideShare.API/RideShare.API.csproj"
COPY . .
WORKDIR "/RideShare.API"
RUN dotnet build "RideShare.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "RideShare.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "RideShare.API.dll"]