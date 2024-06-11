POST /api/Contract/Upload HTTP/1.1
Accept-Encoding: gzip, deflate
Accept-Language: ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7
Connection: keep-alive
Content-Length: 19475
Content-Type: multipart/form-data; boundary=----WebKitFormBoundaryhAwBcwsa0wjj6RT1
Cookie: rxVisitor=17180838583380991J9H8AKP1UHHO39BKQ0MALJV99T0F; dtSa=-; dtCookie=v_4_srv_3_sn_9ENOL3PPCC9FA7AHQ1D8KRDIKEC6CVL2_perc_100000_ol_0_mul_1_app-3Aea7c4b59f27d43eb_1_rcs-3Acss_0; dtPC=3$283858336_446h-vRECERAJLNUIFPHQGOTOTKTMJBDEEMJIK-0e0; rxvt=1718103778200|1718101978200
Host: dspm2a001.halykbank.nb:11021
Origin: http://dspm2a001.halykbank.nb:11021
Referer: http://dspm2a001.halykbank.nb:11021/index.html
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36
accept: */*

это запрос со свагера сервиса куда я обращаюсь и ниже мой запрос 

{
  "Version": "1.1",
  "Content": {
    "Headers": [
      {
        "Key": "Content-Length",
        "Value": [
          "0"
        ]
      }
    ]
  },
  "StatusCode": 500,
  "ReasonPhrase": "Internal Server Error",
  "Headers": [
    {
      "Key": "Date",
      "Value": [
        "Tue, 11 Jun 2024 12:06:47 GMT"
      ]
    },
    {
      "Key": "Server",
      "Value": [
        "Kestrel"
      ]
    }
  ],
  "TrailingHeaders": [
    
  ],
  "RequestMessage": {
    "Version": "1.1",
    "VersionPolicy": 0,
    "Content": [
      {
        "Headers": [
          {
            "Key": "Content-Type",
            "Value": [
              "application/pdf"
            ]
          },
          {
            "Key": "Content-Disposition",
            "Value": [
              "form-data"
            ]
          }
        ]
      },
      {
        "Headers": [
          {
            "Key": "Content-Type",
            "Value": [
              "text/plain; charset=utf-8"
            ]
          },
          {
            "Key": "Content-Disposition",
            "Value": [
              "form-data"
            ]
          }
        ]
      }
    ],
    "Method": {
      "Method": "POST"
    },
    "RequestUri": "http://dspm2a001.halykbank.nb:11021/api/Contract/Upload",
    "Headers": [
      {
        "Key": "traceparent",
        "Value": [
          "00-eb6d224e1f7bde2c420de8de5050f909-0c7b7e7329a49a28-00"
        ]
      }
    ],
    "Properties": {
      
    },
    "Options": {
      
    }
  },
  "IsSuccessStatusCode": false
}
