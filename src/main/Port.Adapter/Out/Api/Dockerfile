#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["main/Port.Adapter/Out/Api/Out.Api.csproj", "main/Port.Adapter/Out/Api/"]
RUN dotnet restore "main/Port.Adapter/Out/Api/Out.Api.csproj"
COPY . .
WORKDIR "/src/main/Port.Adapter/Out/Api"
RUN dotnet build "Out.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Out.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ei8.Cortex.Subscriptions.Out.Api.dll"]