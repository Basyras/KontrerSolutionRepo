#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Kontrer.OwnerSever.MessageBus.HttpProxy/Kontrer.OwnerSever.MessageBus.HttpProxy.csproj", "Kontrer.OwnerSever.MessageBus.HttpProxy/"]
RUN dotnet restore "Kontrer.OwnerSever.MessageBus.HttpProxy/Kontrer.OwnerSever.MessageBus.HttpProxy.csproj"
COPY . .
WORKDIR "/src/Kontrer.OwnerSever.MessageBus.HttpProxy"
RUN dotnet build "Kontrer.OwnerSever.MessageBus.HttpProxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kontrer.OwnerSever.MessageBus.HttpProxy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kontrer.OwnerSever.MessageBus.HttpProxy.dll"]