if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(swag => 
    {
        swag.SwaggerEndpoint("/swagger/v1/swagger.json", "TemplateExample");
        swag.RoutePrefix = "swagger/swagger.json";
    });
}

            builder.Services.AddSwaggerGen(s => 
            {
                s.SwaggerDoc("TemplateExample", new OpenApiInfo
                {
                    Title = "TemplateExample",
                    Version = "v1"
                });
            });

            я добавил такие хуйнюшки, но проект открывается на ссылку https://localhost:44355/swagger
