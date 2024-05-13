appsettings.json:
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "Cardcolv": ""
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Error",
        "System": "Warning"
      }
    }
  },
  "ExternalServices": {
    "HHDSoapService": "",
    "Colvir": ""
  },
  "AllowedHosts": "*",
  "ApiKey": ""
}

appsettings.Development.json:
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=ala720i01.halykbank.nb)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=SPMDEV)));User Id=cardcolv;Password=t1est34"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Error",
        "System": "Warning"
      }
    }
  },
  "ExternalServices": {
    "HHDSoapService": "http://IRIS2A001.test.dev:57772/csp/hddmain/Service.GetFinToolDocsMSB.cls",
    "Colvir": "http://colvirbs-credits.service.test-dc.consul"
  },
  "AllowedHosts": "*",
  "ApiKey": {
    "DefaultKey": "DefaultKey"
  }
}

launchSettings.json:
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:50584",
      "sslPort": 44376
    }
  },
  "profiles": {
    "SPM3._0Service": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      },
      "applicationUrl": "https://localhost:7230;http://localhost:5230",
      "dotnetRunMessages": true
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "Docker": {
      "commandName": "Docker",
      "launchBrowser": true,
      "launchUrl": "{Scheme}://{ServiceHost}:{ServicePort}/swagger",
      "publishAllPorts": true,
      "useSSL": true
    }
  }
}
