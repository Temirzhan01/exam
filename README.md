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

  {
    "version": {
        "major": 1,
        "minor": 1,
        "build": -1,
        "revision": -1,
        "majorRevision": -1,
        "minorRevision": -1
    },
    "content": {
        "headers": [
            {
                "Key": "Content-Type",
                "Value": [
                    "multipart/form-data; boundary=\"b375689a-1f7b-4029-bd6c-f459ed2180b1\""
                ]
            },
            {
                "Key": "Content-Length",
                "Value": [
                    "19425"
                ]
            }
        ]
    },
    "method": {
        "method": "POST"
    },
    "requestUri": "http://172.16.143.9:8069/AtStorFilial/Upload.ashx?docID=4D65574C6768365539695176644A6E4F4449664330773D3D",
    "headers": [
        {
            "Key": "X-dynaTrace",
            "Value": [
                "FW4;-1076712821;1;91770045;964;1;62323150;327;95d1;2h01;3h05784cbd;4h03c4;6h2c7b1df9a8ba7fb8676b9b8bd6f6f0f3;7ha5fc7aebc403d45d"
            ]
        },
        {
            "Key": "traceparent",
            "Value": [
                "00-2c7b1df9a8ba7fb8676b9b8bd6f6f0f3-a5fc7aebc403d45d-01"
            ]
        },
        {
            "Key": "tracestate",
            "Value": [
                "3b6f9ce-bfd2aa8b@dt=fw4;1;5784cbd;3c4;1;0;0;147;e5f8;2h01;3h05784cbd;4h03c4;7ha5fc7aebc403d45d"
            ]
        },
        {
            "Key": "Request-Id",
            "Value": [
                "|944967f8-42d856a792241f16.2."
            ]
        }
    ],
    "properties": {}
}
}

это верный запрос со свагера сервиса куда я обращаюсь и ниже мой запрос 

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



