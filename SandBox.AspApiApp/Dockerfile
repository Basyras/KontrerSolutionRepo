#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SandBox.AspApiApp/SandBox.AspApiApp.csproj", "SandBox.AspApiApp/"]
RUN dotnet restore "SandBox.AspApiApp/SandBox.AspApiApp.csproj"
COPY . .
WORKDIR "/src/SandBox.AspApiApp"
RUN dotnet build "SandBox.AspApiApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SandBox.AspApiApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SandBox.AspApiApp.dll"]