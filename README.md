            errors = report.Entries.Select(x => new { key = x.Key, value = x.Value.Description ?? x.Value.Exception?.Message }).Where(x => x.value != null)
