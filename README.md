System.IO.DirectoryNotFoundException: Could not find a part of the path '/app/..\..\..\/HTMLDocuments/Outcome.html'.

        private async Task<string> LoadHtmlTemplate(string templateName)
        {
            string fp = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\")), "HTMLDocuments", $"{templateName}.html");
            return await System.IO.File.ReadAllTextAsync(fp);
        }
