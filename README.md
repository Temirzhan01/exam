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
                using (HttpResponseMessage response = await client.PostAsJsonAsync($"finish/task", taskData))
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

        Ответ - Internal server error Response status code does not indicate success: 500 (Internal Server Error).
        А это модель taskData:
        "{\"resultActionCode\":\"Отправить\",\"taskModel\":\"{\\\"processType\\\":198,\\\"bankEmployee\\\":{\\\"login\\\":null,\\\"lastName\\\":\\\"Online\\\",\\\"firstName\\\":\\\"Bank\\\",\\\"middleName\\\":\\\" \\\",\\\"tabNumber\\\":null,\\\"branch\\\":null,\\\"tUserId\\\":null,\\\"eMail\\\":null,\\\"roles\\\":[],\\\"addInfo\\\":\\\"Online Bank \\\",\\\"fullName\\\":\\\"Online Bank  \\\",\\\"adLogin\\\":null},\\\"isOnlineBank\\\":true,\\\"isHomeBank\\\":false,\\\"homebankId\\\":null,\\\"isConfidant\\\":false,\\\"cassOrder\\\":null,\\\"cardNotFound\\\":false,\\\"tcrGetRedy\\\":false,\\\"branch\\\":{\\\"id\\\":null,\\\"parentId\\\":null,\\\"name\\\":null,\\\"number\\\":null,\\\"p_number\\\":null},\\\"showTextWarn\\\":false,\\\"tcrGo\\\":false,\\\"showTcrErr\\\":false,\\\"returned\\\":false,\\\"getStatRes\\\":{\\\"resultCode\\\":0,\\\"errorText\\\":null},\\\"errorCalculate\\\":null,\\\"requestNumber\\\":\\\"100159812910\\\",\\\"requestCreateDate\\\":\\\"2023-06-23T16:58:52\\\",\\\"clientInfo\\\":{\\\"clientCode\\\":null,\\\"clientId\\\":null,\\\"iinBin\\\":\\\"040740008423\\\",\\\"clientName\\\":\\\"050201.156994\\\",\\\"documentType\\\":null,\\\"documentNumber\\\":null,\\\"documentIssuer\\\":null,\\\"documentIssueDate\\\":null,\\\"isResident\\\":false,\\\"birthDate\\\":null,\\\"address\\\":null,\\\"citizenshipCountry\\\":null,\\\"searchResult\\\":null,\\\"firstNameHead\\\":null,\\\"lastNameHead\\\":null,\\\"middleNameHead\\\":null,\\\"iinHead\\\":null,\\\"kod\\\":null,\\\"isClientCardExists\\\":false,\\\"isSearchDone\\\":false,\\\"residentCountryCode\\\":null},\\\"recycler\\\":{\\\"recCode\\\":null,\\\"recName\\\":null,\\\"recLocation\\\":null,\\\"recStatus\\\":null,\\\"recType\\\":null,\\\"kztAmount\\\":null,\\\"usdAmount\\\":null,\\\"eurAmount\\\":null,\\\"depositoryCode\\\":null,\\\"depositoryName\\\":null},\\\"withdrawRes\\\":{\\\"errorCode\\\":null,\\\"errorText\\\":null,\\\"transactionId\\\":null,\\\"reqNum\\\":null,\\\"recAcc\\\":null,\\\"recAccExt\\\":null,\\\"vmix\\\":null,\\\"sesId\\\":null,\\\"recordId\\\":null},\\\"rrko\\\":null,\\\"rrkodate\\\":\\\"2023-06-23T00:00:00\\\",\\\"transaction\\\":{\\\"account\\\":\\\"KZ816017131000002212\\\",\\\"accountId\\\":null,\\\"accountDepartmentId\\\":null,\\\"accountAmount\\\":null,\\\"accountAviableAmount\\\":null,\\\"accountCurrency\\\":\\\"KZT\\\",\\\"accountType\\\":null,\\\"cardId\\\":null,\\\"excess\\\":null,\\\"analytics\\\":null,\\\"settlement\\\":false,\\\"transactionAmount\\\":\\\"3000\\\",\\\"transactionActualAmount\\\":null,\\\"transactionCommission\\\":\\\"250\\\",\\\"sessionId\\\":null,\\\"tcrTransactionId\\\":null,\\\"recyclerId\\\":null,\\\"recyclerName\\\":null,\\\"recyclerCity\\\":null,\\\"invoiceAmount\\\":\\\"46985614.2\\\",\\\"eknp\\\":null,\\\"operDate\\\":\\\"0001-01-01T00:00:00\\\",\\\"operId\\\":null,\\\"reqID\\\":\\\"1930744\\\",\\\"key\\\":\\\"202703\\\",\\\"reqNum\\\":null,\\\"kod\\\":\\\"15\\\",\\\"kbe\\\":{\\\"code\\\":\\\"15\\\",\\\"displayName\\\":null},\\\"knp\\\":{\\\"code\\\":\\\"341\\\",\\\"displayName\\\":null},\\\"skp\\\":{\\\"code\\\":\\\"21\\\",\\\"displayName\\\":null},\\\"destination\\\":\\\"Снятие наличных денег с текущих или корреспондентских счетов,\\\",\\\"branch\\\":null,\\\"differenceSumm\\\":null,\\\"difference\\\":false,\\\"differenceType\\\":null,\\\"clientType\\\":0},\\\"summAmount\\\":null,\\\"setAccount\\\":{\\\"iban\\\":null,\\\"currency\\\":null,\\\"currencyName\\\":null,\\\"amount\\\":null,\\\"depCode\\\":null,\\\"depId\\\":null,\\\"accountSearchResult\\\":null,\\\"status\\\":null,\\\"type\\\":null},\\\"setExtAccount\\\":{\\\"iban\\\":null,\\\"currency\\\":null,\\\"currencyName\\\":null,\\\"amount\\\":null,\\\"depCode\\\":null,\\\"depId\\\":null,\\\"accountSearchResult\\\":null,\\\"status\\\":null,\\\"type\\\":null},\\\"commision\\\":null,\\\"totalComm\\\":null,\\\"firstName\\\":\\\"АЙБЕК\\\",\\\"lastName\\\":\\\"ТУРАБЕКОВ\\\",\\\"middleName\\\":\\\"ЕРМУХАМБЕТОВИЧ\\\",\\\"iin\\\":\\\"961023301570\\\",\\\"documentNumber\\\":\\\"\\\",\\\"documentDate\\\":\\\"0001-01-01T00:00:00\\\",\\\"documentDateString\\\":\\\"\\\",\\\"telephoneNumber\\\":null,\\\"applicantAddress\\\":null,\\\"maxSumCashIn\\\":\\\"2000000\\\",\\\"warrantNumber\\\":null,\\\"status\\\":3,\\\"statusString\\\":\\\"0\\\",\\\"executionHistory\\\":[],\\\"documenExpiretDate\\\":\\\"0001-01-01T00:00:00\\\",\\\"documentExpireDateString\\\":\\\"\\\",\\\"executionResponse\\\":{\\\"resultCode\\\":null,\\\"resultStatus\\\":null,\\\"processId\\\":null},\\\"authResponse\\\":{\\\"code\\\":null,\\\"message\\\":null,\\\"data\\\":null},\\\"finResponse\\\":{\\\"code\\\":null,\\\"message\\\":null,\\\"data\\\":null},\\\"executionId\\\":null,\\\"confidantInfo\\\":{\\\"id\\\":null,\\\"iin\\\":null,\\\"fullName\\\":null,\\\"documentType\\\":null,\\\"documentNumber\\\":null,\\\"documentIssuer\\\":null,\\\"documentIssueDate\\\":null,\\\"powerOfAttorneyNumber\\\":null,\\\"powerOfAttorneyIssueDate\\\":null,\\\"birthDate\\\":null,\\\"address\\\":null,\\\"citizenshipCountry\\\":null},\\\"userCassInfo\\\":null,\\\"timeoutMinutes\\\":0,\\\"rkodateString\\\":\\\"23.06.2023\\\"}\",\"taskId\":\"100394948\",\"processId\":\"100159812910\",\"bpVersionId\":\"1138\",\"bpRoleCode\":\"376\"}"

входящие параметры сторонного сервиса:
{
  "taskModel": "string",
  "taskId": "string",
  "processId": "string",
  "resultActionCode": "string",
  "bpVersionId": "string",
  "bpRoleCode": "string"
}
