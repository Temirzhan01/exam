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
            Type genericType = typeof(IDrawDocumentService<>).MakeGenericType(type); 
            return _serviceProvider.GetService(genericType);
        }
    }

    все равно ошибка
