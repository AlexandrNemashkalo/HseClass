#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 443 80


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HseClass.Api/HseClass.Api.csproj", "HseClass.Api/"]
COPY ["HseClass.Data/HseClass.Data.csproj", "HseClass.Data/"]
COPY ["HseClass.Core/HseClass.Core.csproj", "HseClass.Core/"]
RUN dotnet restore "HseClass.Api/HseClass.Api.csproj"
COPY . .
WORKDIR "/src/HseClass.Api"
RUN dotnet build "HseClass.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HseClass.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HseClass.Api.dll"]
