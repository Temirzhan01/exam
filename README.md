FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV TZ=Asia/Almaty
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone 

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Template/Template.csproj", "Template/"]
COPY ["Template/nuget.config", "."]

ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS='{"endpointCredentials": [{"endpoint":"http://172.28.64.23:8081/repository/nuget-group/"}]}'
RUN dotnet restore "Template/Template.csproj"
COPY . .
WORKDIR "/src/Template"
RUN dotnet build "Template.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Template.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Template.dll"]


У меня не проходит билд в пайплайне, хотя локально доукер билдится, 

ниже нугетконфиг мой, и ошибка

<?xml version="1.0" encoding="utf-8"?>
<configuration>
	<packageSources>
		<add key="customfeed" value="http://172.28.64.23:8081/repository/nuget-group/" />
	</packageSources>
</configuration>

#14 [build 5/8] RUN dotnet restore "Template/Template.csproj"
#14 1.539   Determining projects to restore...
#14 2.242 /src/Template/Template.csproj : warning NU1803: You are running the 'restore' operation with an 'HTTP' source, 'http://172.28.64.23:8081/repository/nuget-group/'. Non-HTTPS access will be removed in a future version. Consider migrating to an 'HTTPS' source.
#14 200.0 /src/Template/Template.csproj : error NU1301: Unable to load the service index for source https://api.nuget.org/v3/index.json.
#14 397.8 /src/Template/Template.csproj : error NU1301: Unable to load the service index for source https://api.nuget.org/v3/index.json.
#14 595.6 /src/Template/Template.csproj : error NU1301: Unable to load the service index for source https://api.nuget.org/v3/index.json.



структура проекта примерно такая 


git
sln
dockerfile
directory: Template{
  mainproj.csproj
  nuget.config
}
