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
ENTRYPOINT ["dotnet", "LegalCashOperationsWorker.dll"]


{
  "profiles": {
    "LegalCashOperationsWorker": {
      "commandName": "Project",
      "environmentVariables": {
        "DOTNET_ENVIRONMENT": "Development",
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "dotnetRunMessages": true
    },
    "Docker": {
      "commandName": "Docker",
      "useSSL": true
    }
  }
}
