#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Catalog/Microservices.Kata.Catalog.API/Microservices.Kata.Catalog.API.csproj", "Services/Catalog/Microservices.Kata.Catalog.API/"]
RUN dotnet restore "Services/Catalog/Microservices.Kata.Catalog.API/Microservices.Kata.Catalog.API.csproj"
COPY . .
WORKDIR "/src/Services/Catalog/Microservices.Kata.Catalog.API"
RUN dotnet build "Microservices.Kata.Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservices.Kata.Catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.Kata.Catalog.API.dll"]