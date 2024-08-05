public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    
    // Добавление Swagger генерации документации
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo 
        { 
            Title = "My API", 
            Version = "v1" 
        });
    });
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    // Включение middleware для генерации JSON документации
    app.UseSwagger();

    // Включение middleware для Swagger UI
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

        // Настройка кастомного URL для Swagger UI
        c.RoutePrefix = "api-docs";  // URL для доступа к Swagger UI будет http://localhost:5000/api-docs
    });

    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

    // Настройка кастомного URL для Swagger UI
    c.RoutePrefix = "api-docs";  // URL для доступа к Swagger UI будет http://localhost:5000/api-docs
});

c.RoutePrefix = string.Empty;
