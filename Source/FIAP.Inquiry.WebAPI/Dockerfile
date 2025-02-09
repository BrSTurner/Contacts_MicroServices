#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Source/FIAP.Inquiry.WebAPI/FIAP.Inquiry.WebAPI.csproj", "Source/FIAP.Inquiry.WebAPI/"]
COPY ["Source/FIAP.Inquiry.Application/FIAP.Inquiry.Application.csproj", "Source/FIAP.Inquiry.Application/"]
COPY ["Source/FIAP.MessageBus/FIAP.MessageBus.csproj", "Source/FIAP.MessageBus/"]
COPY ["Source/FIAP.SharedKernel/FIAP.SharedKernel.csproj", "Source/FIAP.SharedKernel/"]
RUN dotnet restore "./Source/FIAP.Inquiry.WebAPI/FIAP.Inquiry.WebAPI.csproj"
COPY . .
WORKDIR "/src/Source/FIAP.Inquiry.WebAPI"
RUN dotnet build "./FIAP.Inquiry.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FIAP.Inquiry.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.Inquiry.WebAPI.dll"]