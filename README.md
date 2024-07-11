FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV TZ=Asia/Almaty
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone 
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SstlService/SstlService.csproj", "SstlService/"]
RUN dotnet restore "SstlService/SstlService.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "./SstlService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./SstlService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SstlService.dll"]


Ошибка при сборке

Dockerfile:12
--------------------
  10 |     FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
  11 |     WORKDIR /src
  12 | >>> COPY ["SstlService/SstlService.csproj", "SstlService/"]
  13 |     RUN dotnet restore "SstlService/SstlService.csproj"
  14 |     COPY . .
--------------------
ERROR: failed to solve: failed to compute cache key: failed to calculate checksum of ref cb8e8101-b05c-45c3-9eb2-533342da1eed::k1dla4ub9rdjz63z73ir8sy9j: "/SstlService/SstlService.csproj": not found
