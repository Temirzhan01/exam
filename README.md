using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;

public class DocumentGenerator
{
    private Document _document;
    private Section _section;
    private Client _client;
    private MdView _mdv;

    public DocumentGenerator(Client client, MdView mdv)
    {
        _client = client;
        _mdv = mdv;
        _document = new Document();
        _section = _document.AddSection();
    }

    public void DrawDocument()
    {
        Table table = _section.AddTable();
        table.Format.Alignment = ParagraphAlignment.Right;
        table.AddColumn(WidthAsPercent(100));
        table.Format.Font.Bold = true;
        table.LeftPadding = Unit.FromCentimeter(0);

        Row currentRow = table.AddRow();
        string date = DateTime.Now.ToString("HH:mm:ss - dd.MM.yyyy года");
        currentRow.Cells[0].AddParagraph($"Дата и время запроса {date}");
        table.AddRow();

        currentRow = table.AddRow();
        currentRow.Format.Font.Bold = true;
        currentRow.Cells[0].AddParagraph("РЕЗУЛЬТАТЫ ПРОВЕРКИ ПО ВНУТРЕННИМ БАЗАМ");
        currentRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
        currentRow.Cells[0].Format.Font.Size = 12;

        Table table1 = new Table();
        table1.Format.Alignment = ParagraphAlignment.Right;
        table1.AddColumn(WidthAsPercent(100));
        currentRow.Cells[0].Elements.Add(table1);

        var currentRow1 = table1.AddRow();

        switch (_mdv.Checked_Person)
        {
            case "Client":
                currentRow1.Cells[0].AddParagraph($"Заемщик {_client.CliName}").Format.Font.Bold = true;
                break;
            case "Guarantor":
                currentRow1.Cells[0].AddParagraph($"Гарант {_client.CliName}").Format.Font.Bold = true;
                break;
            case "Warrantor":
                currentRow1.Cells[0].AddParagraph($"Поручитель {_client.CliName}").Format.Font.Bold = true;
                break;
            case "Pledger":
                currentRow1.Cells[0].AddParagraph($"Залогодатель {_client.CliName}").Format.Font.Bold = true;
                break;
            case "Codebtor":
                currentRow1.Cells[0].AddParagraph($"Созаемщик {_client.CliName}").Format.Font.Bold = true;
                break;
        }

        if (_client.CliJurFl == 1)
        {
            currentRow1.Cells[0].AddParagraph($"БИН - {_client.CliBinIin}").Format.Font.Bold = false;
        }
        else
        {
            currentRow1.Cells[0].AddParagraph($"ИИН - {_client.CliBinIin}").Format.Font.Bold = false;
        }

        // Продолжение формирования документа...

        // Пример добавления дополнительных разделов и строк
        AddInternalDbCheckResults(table);

        // Сохранение документа
        PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
        renderer.Document = _document;
        renderer.RenderDocument();
        renderer.Save("InternalDbCheckResults.pdf");
    }

    private void AddInternalDbCheckResults(Table table)
    {
        AddSection(table, "БД 'Террористов и террористических организаций'");
        AddRow(table, "Статус проверки:", "Ошибка");
        AddRow(table, "Результат поиска:", "Не найден");
        AddRow(table, "Комментарий:", "Ошибка");

        AddSection(table, "БД 'Лиц, связанных с Банком особыми отношениями'");
        AddRow(table, "Статус проверки:", "Ошибка");
        AddRow(table, "Результат поиска:", "Не найден");
        AddRow(table, "Комментарий:", "ORA-00604: ошибка на рекурсивном SQL-уровне 1 ORA-01882: область часового пояса не найдена");

        AddSection(table, "БД 'Взаимосвязанных между собой компаний - клиентов'");
        AddRow(table, "Статус проверки:", "Ошибка");
        AddRow(table, "Результат поиска:", "Не найден");
        AddRow(table, "Комментарий:", "ORA-00604: ошибка на рекурсивном SQL-уровне 1 ORA-01882: область часового пояса не найдена");

        AddSection(table, "ИНФОРМАЦИЯ ПО ТЕКУЩИМ СЧЕТАМ КЛИЕНТА");
        AddSection(table, "ИНФОРМАЦИЯ В РОЗНИЧНОЙ АБС");
        AddRow(table, "Статус проверки:", "Проверено");
        AddRow(table, "Результат поиска:", "Не найден");
        AddRow(table, "Комментарий:", "");

        AddSection(table, "ИНФОРМАЦИЯ В КОРПОРАТИВНОЙ АБС");
        AddRow(table, "Статус проверки:", "Проверено");
        AddRow(table, "Результат поиска:", "Найден");
        AddRow(table, "Комментарий:", "");
    }

    private void AddRow(Table table, string label, string value)
    {
        var row = table.AddRow();
        row.Cells[0].AddParagraph($"{label} {value}");
    }

    private void AddSection(Table table, string sectionTitle)
    {
        var row = table.AddRow();
        var cell = row.Cells[0];
        var paragraph = cell.AddParagraph(sectionTitle);
        paragraph.Format.Font.Bold = true;
        paragraph.Format.Font.Size = 10;
        cell.MergeRight = 1;
    }

    private Unit WidthAsPercent(double percent)
    {
        return Unit.FromCentimeter(20 * (percent / 100));
    }
}

// Пример использования
public class Client
{
    public string CliName { get; set; }
    public string CliBinIin { get; set; }
    public int CliJurFl { get; set; }
}

public class MdView
{
    public string Checked_Person { get; set; }
}

public static class Program
{
    public static void Main()
    {
        var client = new Client { CliName = "Example Client", CliBinIin = "123456789", CliJurFl = 1 };
        var mdv = new MdView { Checked_Person = "Client" };

        var generator = new DocumentGenerator(client, mdv);
        generator.DrawDocument();
    }
}
