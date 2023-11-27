        public HttpClient _client 
        {
            get 
            {
                var httpClientHandler = new HttpClientHandler()
                {
                    Proxy = new WebProxy(),
                    UseDefaultCredentials = true
                };
                HttpClient client = new HttpClient(httpClientHandler);
                client.MaxResponseContentBufferSize = int.MaxValue;
                var request = new HttpRequestMessage(new HttpMethod("GET"), "https://halykbpm-auth.halykbank.nb/win-Auth/jwt/bearer?clientId=bp-api");
                var result = client.SendAsync(request).Result;
                if (result.IsSuccessStatusCode) 
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Content.ReadAsStringAsync().Result.ToString());
                }
                return client;
            }
        }
