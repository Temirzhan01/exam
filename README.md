{
  "appSettings": {
    "service.targetPort": "80",
    "probes.enabled": "false",
    "resources.limits.memory": "500Mi",
    "resources.limits.cpu": "300m"
  },
  "appName": "legalcashoperationsworker",
  "appType": "integration",
  "appRepositoryName": "halykbpm-git.homebank.kz/spm-business-process/LegalCashOperationsWorker",
  "appBranch": "develop",
  "enviromentType": "develop",
  "dockerfileDefault": false,
  "dotnetConfigDir": "appsettings.Development.json",
  "containerName": "legalcashoperationsworker.develop"
}

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


и конец консоли 
Scheduling project: spm » deploy
Starting building: spm » deploy #77
[Pipeline] }
spm » deploy #77 completed with status FAILURE (propagate: false to ignore)
Retrying
[Pipeline] {
[Pipeline] build (Building spm » deploy)
Scheduling project: spm » deploy
Starting building: spm » deploy #78
[Pipeline] }
[Pipeline] // retry
[Pipeline] }
[Pipeline] // script
[Pipeline] }
[Pipeline] // stage
[Pipeline] stage
[Pipeline] { (Declarative: Post Actions)
[Pipeline] archiveArtifacts
Archiving artifacts
[Pipeline] script
[Pipeline] {
[Pipeline] echo
Manual start job fail
[Pipeline] }
[Pipeline] // script
[Pipeline] }
[Pipeline] // stage
[Pipeline] }
[Pipeline] // withEnv
[Pipeline] }
[Pipeline] // node
[Pipeline] End of Pipeline
spm » deploy #78 completed with status FAILURE (propagate: false to ignore)
Finished: FAILURE
