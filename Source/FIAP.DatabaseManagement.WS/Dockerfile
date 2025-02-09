#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Source/FIAP.DatabaseManagement.WS/FIAP.DatabaseManagement.WS.csproj", "Source/FIAP.DatabaseManagement.WS/"]
COPY ["Source/FIAP.DatabaseManagement/FIAP.DatabaseManagement.csproj", "Source/FIAP.DatabaseManagement/"]
COPY ["Source/FIAP.SharedKernel/FIAP.SharedKernel.csproj", "Source/FIAP.SharedKernel/"]
COPY ["Source/FIAP.MessageBus/FIAP.MessageBus.csproj", "Source/FIAP.MessageBus/"]
RUN dotnet restore "./Source/FIAP.DatabaseManagement.WS/FIAP.DatabaseManagement.WS.csproj"
COPY . .
WORKDIR "/src/Source/FIAP.DatabaseManagement.WS"
RUN dotnet build "./FIAP.DatabaseManagement.WS.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FIAP.DatabaseManagement.WS.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.DatabaseManagement.WS.dll"]