        public async Task<string> ChangeStatus(string reqNumber, int status)
        {
            try
            {
                TaskData taskData = await GetTaskData(reqNumber);
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(config.Ensemble);
                var intermediate = JsonConvert.DeserializeObject<TaskModel>(taskData.taskModel);
                intermediate.status = status;
                taskData.taskModel = JsonConvert.SerializeObject(intermediate);
                var body = JsonConvert.SerializeObject(taskData);
                using (HttpResponseMessage response = await client.SendAsync( new HttpRequestMessage(HttpMethod.Post, $"finish/task") { Content = new StringContent(body, Encoding.UTF8, "application/json") }))
                {
                    response.EnsureSuccessStatusCode();
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                throw new HttpRequestException(e.Message);
            }
        }
