#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["PlatformService/PlatformService.csproj", "PlatformService/"]
#RUN dotnet restore "PlatformService/PlatformService.csproj"
#COPY . .
#WORKDIR "/src/PlatformService"
#RUN dotnet build "PlatformService.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "PlatformService.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "PlatformService.dll"]

#    mcr.microsoft.com/dotnet/sdk:7.0-alpine
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

EXPOSE 80
EXPOSE 443

COPY . ./
RUN dotnet publish "PlatformService.csproj" -c Release -o out /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS build

EXPOSE 80
EXPOSE 443

WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "PlatformService.dll"]