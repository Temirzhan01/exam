Это у нас проект рабочий, в нем работает
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromMinutes(5);
                    var response = client.PostAsJsonAsync(ConfigurationManager.AppSettings["colvir"] + $"/api/oracle/GetCredQueueResult", request).Result;
                    return response;
                }

                А это мое, не рабочий 504 выдает
        public async Task<LoanResult> GetResultLoan(string refer)
        {
            LoanResult loanResult = new LoanResult();
            HttpClient client = _httpClientFactory.CreateClient("Colvir");
            var body = JsonConvert.SerializeObject(new LoanResultRequest() { Refer = refer });
            client.DefaultRequestHeaders.Accept.Clear();
            client.Timeout = TimeSpan.FromMinutes(5);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                using (HttpResponseMessage response = await client.PostAsJsonAsync("/api/oracle/GetCredQueueResult", body))  //SendAsync(new HttpRequestMessage(HttpMethod.Post, $"/api/oracle/GetCredQueueResult") { Content = new StringContent(body) }))
                {
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsStringAsync();
                    var jsonloanResult = JsonConvert.DeserializeObject<LoanResultResponse>(result);
                    loanResult = JsonConvert.DeserializeObject<LoanResult>(jsonloanResult.list);
                    _logger.LogInformation(jsonloanResult.list);
                }
            }
            catch (Exception ex) 
            {
                loanResult.status = "1";
                loanResult.sResult = ex.ToString();
                _logger.LogError("GetResultLoan-> " + ex.ToString());
            }
            return loanResult;
        }
