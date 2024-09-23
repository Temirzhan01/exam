if (builder.Configuration["Environment"] == "Development")
{
    string basePath = string.Empty;
    app.UseSwagger(c => 
    {
        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            basePath = httpReq.PathBase.HasValue ? httpReq.PathBase.Value : string.Empty;
            swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{basePath}" } };
        });
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"{basePath}/swagger/v1/swagger.json", "API");
        c.RoutePrefix = basePath;
    });
}
