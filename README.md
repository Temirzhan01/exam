2024-05-14 20:14:53 HOSTNAME=b30b1b350dd7
2024-05-14 20:14:53 HOME=/root
2024-05-14 20:14:53 DOTNET_RUNNING_IN_CONTAINER=true
2024-05-14 20:14:53 DOTNET_VERSION=6.0.29
2024-05-14 20:14:53 ASPNETCORE_ENVIRONMENT=Development
2024-05-14 20:14:53 PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin
2024-05-14 20:14:53 ASPNETCORE_URLS=http://+:80
2024-05-14 20:14:53 PWD=/app
2024-05-14 20:14:53 TZ=Asia/Almaty
2024-05-14 20:14:53 ASPNET_VERSION=6.0.29
2024-05-14 20:14:53 ASPNETCORE_ENVIRONMENT=Development
2024-05-14 20:14:53 ASPNETCORE_ENVIRONMENT: Production


FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV TZ=Asia/Almaty
ENV ASPNETCORE_ENVIRONMENT=Development
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone 

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LegalCashOperationsWorker.csproj", "."]
RUN dotnet restore "./LegalCashOperationsWorker.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "LegalCashOperationsWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LegalCashOperationsWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

CMD printenv && echo "ASPNETCORE_ENVIRONMENT=$ASPNETCORE_ENVIRONMENT" && dotnet LegalCashOperationsWorker.dll
