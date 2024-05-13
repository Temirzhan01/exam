You must install or update .NET to run this application.

App: /app/LegalCashOperationsWorker.dll
Architecture: x64
Framework: 'Microsoft.AspNetCore.App', version '6.0.0' (x64)
.NET location: /usr/share/dotnet/

No frameworks were found.

Learn about framework resolution:
https://aka.ms/dotnet/app-launch-failed

To install missing framework, download:
https://aka.ms/dotnet-core-applaunch?framework=Microsoft.AspNetCore.App&framework_version=6.0.0&arch=x64&rid=debian.11-x64

Это ошибка Kenkins/Docker, проекта типа Worker, который был создан на основе .net 6. Есть и другие проекты на .net 6, но уже типа api в них в зависимостях есть фреймворк Microsoft.AspNetCore.App но в этом проекте его нет, только Microsoft.NETCore.App
Ниже докерфайла воркера, помоги. 

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
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
