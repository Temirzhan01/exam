        public async Task<LoanResult> GetResultLoan(string refer)
        {
            LoanResult loanResult = new LoanResult();
            HttpClient client = _httpClientFactory.CreateClient("Colvir");
            var body = JsonConvert.SerializeObject(new LoanResultRequest() { Refer = refer });
            try
            {
                using (HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, $"/api/oracle/GetCredQueueResult") { Content = new StringContent(body, Encoding.UTF8, "application/json") }))
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
