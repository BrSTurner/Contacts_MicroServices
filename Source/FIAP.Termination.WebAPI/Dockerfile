#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Source/FIAP.Termination.WebAPI/FIAP.Termination.WebAPI.csproj", "Source/FIAP.Termination.WebAPI/"]
COPY ["Source/FIAP.SharedKernel/FIAP.SharedKernel.csproj", "Source/FIAP.SharedKernel/"]
COPY ["Source/FIAP.Termination.Application/FIAP.Termination.Application.csproj", "Source/FIAP.Termination.Application/"]
COPY ["Source/FIAP.MessageBus/FIAP.MessageBus.csproj", "Source/FIAP.MessageBus/"]
RUN dotnet restore "./Source/FIAP.Termination.WebAPI/FIAP.Termination.WebAPI.csproj"
COPY . .
WORKDIR "/src/Source/FIAP.Termination.WebAPI"
RUN dotnet build "./FIAP.Termination.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FIAP.Termination.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.Termination.WebAPI.dll"]