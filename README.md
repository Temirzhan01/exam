            builder.Services.AddScoped<IDocCreatorService, DocCreatorService>();
            builder.Services.AddScoped(typeof(IDocCreatorService) , typeof(DocCreatorService<>));
