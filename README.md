            "description": "Discover endpoint #0 is not responding with code in 200...299 range, the current status is Forbidden." Хотя ендпоинт ошибку не выдает


        private readonly ITrancheRepository _transhRepository;
        private readonly HttpClient _colvirClient;
        private readonly ILogger<TrancheService> _logger;
        private readonly IMapper _mapper;
        private readonly GetFinToolDocsMSBSoapClient HDDClient;

        public TrancheService(ITrancheRepository transhRepository, IHttpClientFactory httpClientFactory, ILogger<TrancheService> logger, IMapper mapper, IOptions<ExternalServices> options)
        {
            _transhRepository = transhRepository;
            _colvirClient = httpClientFactory.CreateClient("Colvir");
            _logger = logger;
            _mapper = mapper;
            HDDClient = new GetFinToolDocsMSBSoapClient();
            HDDClient.Endpoint.Address = new System.ServiceModel.EndpointAddress(options.Value.HHDSoapService);
        }
        
        public async Task<IEnumerable<Models.Document>> GetDocumentsFromHDDAsync(string branchNumber)
        {
            using (HDDClient)
            {
                var hddDocs = await HDDClient.GetFinToolDocsAsync(branchNumber);
                var result = _mapper.Map<Models.Document[]>(hddDocs.Body.GetFinToolDocsResult.Documents);
                return result;
            }
        }
