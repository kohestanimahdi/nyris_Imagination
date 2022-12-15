FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Imagination.Server/Imagination.Server.csproj", "src/Imagination.Server/"]
RUN dotnet restore "src/Imagination.Server/Imagination.Server.csproj"
COPY . .
WORKDIR "/src/src/Imagination.Server"
RUN dotnet build "Imagination.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Imagination.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN apt-get update && apt-get install -y apt-utils libgdiplus libc6-dev

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Imagination.Server.dll"]