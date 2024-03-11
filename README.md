{
  "serverPort": 8080,
  "releaseMode": false,
  "serviceTimeoutSeconds": 120,
  "logLevel": "DEBUG",
  "logServiceRequests": false,
  "compraUrl": "https://kompra.kz/api/",
  "ESURL": "http://10.3.41.13:9200/bp-api/_doc",
  "ESComponent" : "compra.integration",
  "compraAuthToken": "test_API_v2",
  "dbUser": "ews-integration",
  "dbPass": "3*35@23af8e748ff8974BEd9!136d9fb",
  "dbName": "ews-integration",
  "dbHost": "172.16.143.230",
  "dbPort": "5432",
  "dbMaxIdleConns": 9,
  "dbMaxOpenConns": 20,
  "dbConnMaxLifetime": 120,
  "dbLogMode": "true"
}

 
 u := is.conf.CompraUrl + "v2/reliability-list?identifier=" + clientIINBIN + "&api-token=" + is.conf.CompraAuthToken пытаюсь так, но получаю 

 
	https://kompra.kz/api/v2/reliability-list?identifier=010904500574&api-token=test_API_v2
