#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["OwnerServer/IdGeneratorService/Presentation/Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi/Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.csproj", "OwnerServer/IdGeneratorService/Presentation/Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi/"]
RUN dotnet restore "OwnerServer/IdGeneratorService/Presentation/Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi/Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.csproj"
COPY . .
WORKDIR "/src/OwnerServer/IdGeneratorService/Presentation/Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi"
RUN dotnet build "Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.dll"]