        public async Task<string> GenerateBusinessKey()
        {
            try
            {
                var response = await _camundaClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, $"request-number-generator/requestnumber?reqType={_options.Value.BKGeneratorType}"));
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<ReqNumberResponse>(await response.Content.ReadAsStringAsync()).requestNumber;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Generating business key error: {ex.Message}");
                throw;
            }

        }

the ssl connection could not be established see inner exception


что это за ошибка? 
