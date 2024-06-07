using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.IO;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    private readonly IConverter _converter;

    public PdfController(IConverter converter)
    {
        _converter = converter;
    }

    [HttpPost]
    public IActionResult CreatePdf([FromBody] YourModel model)
    {
        string htmlContent = LoadHtmlTemplate()
            .Replace("{{Title}}", model.Name)
            .Replace("{{Description}}", model.Description);

        var doc = new HtmlToPdfDocument
        {
            GlobalSettings = {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait
            },
            Objects = {
                new ObjectSettings
                {
                    HtmlContent = htmlContent,
                }
            }
        };

        byte[] pdf = _converter.Convert(doc);

        return File(pdf, "application/pdf", "generated.pdf");
    }

    private string LoadHtmlTemplate()
    {
        // Здесь путь к вашему HTML-шаблону
        return System.IO.File.ReadAllText("template.html");
    }
}
