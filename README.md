Короче, смотри, вот код. Посмотри, изучи и скажи что не так, или же другой способ дай. 

    public static class ServiceExtensions
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IDocCreatorService, DocCreatorService>();
            builder.Services.AddTransient(typeof(IDrawDocumentService<>) , typeof(DrawDocumentService<>));
            builder.Services.AddSingleton<IConverter>(new SynchronizedConverter(new PdfTools()));
            builder.Services.AddScoped<IUploaderService, UploaderService>();
            builder.Services.AddSingleton<IDrawDocumentServiceFactory, DrawDocumentServiceFactory>();
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            builder.Services.AddHealthChecks(builder.Configuration);

            return builder;
        }
    }

    public interface IDrawDocumentServiceFactory
    {
        public Task<object> Create(string type);
    }


    public class DrawDocumentServiceFactory : IDrawDocumentServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public DrawDocumentServiceFactory(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }
        public async Task<object> Create(string typeName) 
        {
            Type type = Type.GetType($"{Assembly.GetExecutingAssembly().GetName().Name}.Models.{typeName}");
            if (type == null) 
            {
                throw new ArgumentNullException(nameof(typeName));
            }
            Type genericType = typeof(IDrawDocumentService<>).MakeGenericType(type); // соль в этом моменте, ошибку выдает
            return _serviceProvider.GetService(genericType);
        }
    }

            [Route("GenerateDocument/{objectType}")]
        [HttpPost]
        public async Task<IActionResult> GenerateDocument(string objectType, [FromBody] JsonElement body)
        {
            var model = JsonSerializer.Serialize(body);
            if (string.IsNullOrEmpty(model))
            {
                return BadRequest("Null data");
            } 
            return Ok(await _DocCreator.GenerateDocument(model, objectType));
        }

            public interface IDocCreatorService
    {
        public Task<byte[]> GenerateDocument(string model, string objectType);
    }
            public async Task<byte[]> GenerateDocument(string model, string objectType) 
        {
            var drawDocumentService = await _factory.Create(objectType);
            var method = drawDocumentService.GetType().GetMethod("DrawDocument");
            return await (Task<byte[]>)method.Invoke(drawDocumentService, new object[] { model, objectType });
        }


            public interface IDrawDocumentService <T>
    {
        public Task<byte[]> DrawDocument(string model, string typeName);
        public Task<HtmlToPdfDocument> BuildFromHTML(T model, string typeName);
    }

    public class DrawDocumentService<T> : IDrawDocumentService<T>
{
    private readonly ILogger<DrawDocumentService<T>> _logger;
    private readonly IConverter _converter;
    private readonly IQrGenerator _generator;

    public DrawDocumentService(ILogger<DrawDocumentService<T>> logger, BasicConverter convertor, IQrGenerator generator)
    {
        _logger = logger;
        _converter = convertor;
        _generator = generator;
    }

    public async Task<byte[]> DrawDocument(string model, string typeName)
    {
        T obj = JsonConvert.DeserializeObject<T>(model);
        return _converter.Convert(await BuildFromHTML(obj, typeName));
    }

    public async Task<HtmlToPdfDocument> BuildFromHTML(T model, string typeName)
    {
        string content = ReplaceFields(model, await LoadHtmlTemplate(typeName));
        return new HtmlToPdfDocument
        {
            GlobalSettings = {
            PaperSize = PaperKind.A4,
            Orientation = Orientation.Portrait
        },
            Objects = {
            new ObjectSettings
            {
                HtmlContent = content,
            }
        }
        };
    }

    private async Task<string> LoadHtmlTemplate(string templateName)
    {
        return await File.ReadAllTextAsync($"{templateName}.html");
    }

    private string ReplaceFields(T model, string content) 
    {
        PropertyInfo[] fields = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (PropertyInfo field in fields) 
        {
            var fieldValue = field.GetValue(model)?.ToString() ?? string.Empty;
            var filedName = field.Name;
            if (field.PropertyType.IsClass && field.PropertyType.Name == "QrCode")
            {
                content.Replace($"{{{{{filedName}}}}}", _generator.GenerateQrCodeBase64(fieldValue));
            }
            else 
            {                
                content.Replace($"{{{{{filedName}}}}}", fieldValue);
            }
        }
        return content;
    }
}

