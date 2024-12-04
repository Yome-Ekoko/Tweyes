##See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#COPY ["TweyesBackend/TweyesBackend.csproj", "TweyesBackend/"]
#COPY ["TweyesBackend.Infrastructure/TweyesBackend.Infrastructure.csproj", "TweyesBackend.Infrastructure/"]
#COPY ["TweyesBackend.Core/TweyesBackend.Core.csproj", "TweyesBackend.Core/"]
#COPY ["TweyesBackend.Persistence/TweyesBackend.Persistence.csproj", "TweyesBackend.Persistence/"]
#COPY ["TweyesBackend.Domain/TweyesBackend.Domain.csproj", "TweyesBackend.Domain/"]
#RUN dotnet restore "TweyesBackend/TweyesBackend.csproj"
#COPY . .
#WORKDIR "/src/TweyesBackend"
#RUN dotnet build "TweyesBackend.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "TweyesBackend.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "TweyesBackend.dll"]