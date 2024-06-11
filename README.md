        public async Task UploadDocument(byte[] doc, string docId) 
        {
            using (var content = new MultipartFormDataContent()) 
            {
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                var byteArrayContent = new ByteArrayContent(doc);
                byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
                content.Add(byteArrayContent);

                var stringContent = new StringContent(docId);
                stringContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                content.Add(stringContent);

                using (HttpResponseMessage response = await _atstoreHDDClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, "api/Contract/Upload") { Content = content }))
                {
                    var a = JsonConvert.SerializeObject(response);
                    response.EnsureSuccessStatusCode();
                    await response.Content.ReadAsStringAsync();
                }
            }
        }

{
  "Version": "1.1",
  "Content": {
    "Headers": [
      {
        "Key": "Content-Type",
        "Value": [
          "application/problem+json; charset=utf-8"
        ]
      }
    ]
  },
  "StatusCode": 400,
  "ReasonPhrase": "Bad Request",
  "Headers": [
    {
      "Key": "Date",
      "Value": [
        "Tue, 11 Jun 2024 13:52:07 GMT"
      ]
    },
    {
      "Key": "Server",
      "Value": [
        "Kestrel"
      ]
    },
    {
      "Key": "Transfer-Encoding",
      "Value": [
        "chunked"
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
          "00-e97be8d52a76c52cf7b8c8a17ad00ce6-6b2488b264fdb558-00"
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
