#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Kontrer.OwnerServer.PricingService.Presentation.AspApi/Kontrer.OwnerServer.PricingService.Presentation.AspApi.csproj", "Kontrer.OwnerServer.PricingService.Presentation.AspApi/"]
RUN dotnet restore "Kontrer.OwnerServer.PricingService.Presentation.AspApi/Kontrer.OwnerServer.PricingService.Presentation.AspApi.csproj"
COPY . .
WORKDIR "/src/Kontrer.OwnerServer.PricingService.Presentation.AspApi"
RUN dotnet build "Kontrer.OwnerServer.PricingService.Presentation.AspApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kontrer.OwnerServer.PricingService.Presentation.AspApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kontrer.OwnerServer.PricingService.Presentation.AspApi.dll"]