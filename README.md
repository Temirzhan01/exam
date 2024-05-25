public override void DrawDocument()
{
    Table table = CurrentSection.AddTable();
    table.Borders.Visible = true;
    table.Borders.Color = Colors.Gray;
    table.Format.Font.Name = "Times New Roman";
    table.Format.Font.Size = 8;
    table.Format.SpaceAfter = 0;
    table.Format.SpaceBefore = 0;

    // Добавляем колонки
    int[] columnWidths = { 80, 100, 40, 55, 80, 55, 100 };
    foreach (var width in columnWidths)
    {
        table.AddColumn(Unit.FromMillimeter(width));
    }

    // Добавляем заголовок
    Row headerRow = table.AddRow();
    headerRow.Cells[0].AddParagraph("БИН/ИИН").Format.Font.Bold = true;
    headerRow.Cells[1].AddParagraph("Наименование").Format.Font.Bold = true;
    headerRow.Cells[2].AddParagraph("Доля").Format.Font.Bold = true;
    headerRow.Cells[3].AddParagraph("ОПФ").Format.Font.Bold = true;
    headerRow.Cells[4].AddParagraph("Parent BIN").Format.Font.Bold = true;
    headerRow.Cells[5].AddParagraph("Level").Format.Font.Bold = true;
    headerRow.Cells[6].AddParagraph("Результаты проверок по базам").Format.Font.Bold = true;

    // Устанавливаем границы для заголовка
    SetCellBorders(headerRow);

    // Добавляем пример строки
    Row row = table.AddRow();
    row.Cells[0].AddParagraph("123456789012");
    row.Cells[1].AddParagraph("Пример компании");
    row.Cells[2].AddParagraph("100%");
    row.Cells[3].AddParagraph("ООО");
    row.Cells[4].AddParagraph("987654321098");
    row.Cells[5].AddParagraph("1");
    row.Cells[6].AddParagraph("Проверено");

    // Устанавливаем границы для строки
    SetCellBorders(row);
}

private void SetCellBorders(Row row)
{
    foreach (var cell in row.Cells)
    {
        cell.Borders.Left.Width = 0.5;
        cell.Borders.Right.Width = 0.5;
        cell.Borders.Top.Width = 0.5;
        cell.Borders.Bottom.Width = 0.5;
        cell.Borders.Left.Color = Colors.Gray;
        cell.Borders.Right.Color = Colors.Gray;
        cell.Borders.Top.Color = Colors.Gray;
        cell.Borders.Bottom.Color = Colors.Gray;
    }
}
