private readonly IConsulClient _consulClient;
        private readonly IConfiguration _configuration;
        public FieldController(IConsulClient consulClient, IConfiguration configuration)
        {
            _consulClient = consulClient;
            _configuration = configuration;
        }

        public ConnectionStringToBase config
        {
            get
            {
                var configData = Task.Run(async () => await _consulClient.KV.Get(_configuration["Environment:ConsulKvName"]));
                return JsonConvert.DeserializeObject<ConnectionStringToBase>(configData.Result.Response.Value.toString());
            }
        }
