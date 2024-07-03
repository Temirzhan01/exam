using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectoryController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public DirectoryController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("list")]
        public IActionResult ListDirectoryContents()
        {
            // Получение пути к корневой папке приложения
            string contentRootPath = _env.ContentRootPath;

            // Путь к папке, содержимое которой нужно вывести
            string directoryPath = Path.Combine(contentRootPath, "HTMLDocuments");

            if (!Directory.Exists(directoryPath))
            {
                return NotFound("Directory not found");
            }

            var files = Directory.GetFiles(directoryPath);
            var directories = Directory.GetDirectories(directoryPath);

            var result = new StringBuilder();
            result.AppendLine("Directories:");
            foreach (var dir in directories)
            {
                result.AppendLine(dir);
            }

            result.AppendLine("Files:");
            foreach (var file in files)
            {
                result.AppendLine(file);
            }

            return Ok(result.ToString());
        }
    }
}
