System.IO.DirectoryNotFoundException: Could not find a part of the path '/app/HTMLDocuments/Outcome.html'.

COPY HTMLDocuments /app/HTMLDocuments/

            return await System.IO.File.ReadAllTextAsync($"HTMLDocuments/{templateName}.html");
