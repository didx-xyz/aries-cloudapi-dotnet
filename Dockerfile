#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["samples/aspnetcore/aspnetcore.csproj", "samples/aspnetcore/"]
COPY ["src/AriesCloudDotnet/AriesCloudDotnet.csproj", "src/AriesCloudDotnet/"]
RUN dotnet restore "samples/aspnetcore/aspnetcore.csproj"
COPY . .
WORKDIR "/src/samples/aspnetcore"
RUN dotnet build "aspnetcore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "aspnetcore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "aspnetcore.dll"]