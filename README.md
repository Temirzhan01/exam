using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = JsonConvert.SerializeObject(new { error = exception.Message });
        return context.Response.WriteAsync(result);
    }
}
    app.UseMiddleware<ExceptionMiddleware>();
public async Task<ActionResult<IEnumerable<ErrorRequest>>> GetAllErrorRequests(string fromDate, string toDate)
{
    // Логирование запроса
    _logger.LogInformation($"FromDate: {fromDate} || Todate: {toDate}");

    // Проверка входных параметров на null или пустоту
    if (string.IsNullOrEmpty(fromDate) || string.IsNullOrEmpty(toDate))
    {
        // Возвращаем ошибку клиенту
        return BadRequest("From date and to date must be provided.");
    }

    // Получение данных
    var errorRequests = await _service.GetAllErrorRequests(fromDate, toDate);

    // Возвращаем результат
    return Ok(errorRequests);
}
