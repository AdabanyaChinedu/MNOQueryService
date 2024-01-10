#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["MNOQueryService.API/MNOQueryService.API.csproj", "MNOQueryService.API/"]
RUN dotnet restore "MNOQueryService.API/MNOQueryService.API.csproj"
COPY . .
WORKDIR "/src/MNOQueryService.API"
RUN dotnet build "MNOQueryService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MNOQueryService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MNOQueryService.API.dll"]