Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
.CreateLogger();

{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Error",
        "System": "Warning"
      }
    }
  },
  "ConnectionStrings": {
    "Cardcolv": ""
  },
  "ExternalServices": {
    "HHDSoapService": "",
    "Colvir": ""
  },
  "AllowedHosts": "*",
  "ApiKey": ""
}


[14:16:41 INF] Request finished HTTP/2 GET https://localhost:7230/api/Tranche/GetAuthorities/gsdgd - - - 401 - - 3.8488ms смотри, мои конфиги, я получаю только логи типа Information, а мне нужны все логи
