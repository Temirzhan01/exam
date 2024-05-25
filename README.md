    public override void DrawDocument()
    {
        Table table = CurrentSection.AddTable();
        table.Format.Alignment = ParagraphAlignment.Right;
        table.AddColumn(WidthAsPercent(100));
        table.Format.Font.Bold = true;
        table.LeftPadding = Unit.FromCentimeter(0);

        Row mainRow = table.AddRow();
        string date = DateTime.Now.ToString("HH:mm:ss - dd.MM.yyyy года");
        mainRow.Cells[0].AddParagraph($"Дата и время запроса {date}");
        mainRow.Cells[0].AddParagraph();

        mainRow = table.AddRow();
        mainRow.Format.Font.Bold = true;
        mainRow.Cells[0].Format.Alignment = ParagraphAlignment.Center;
        mainRow.Cells[0].Format.Font.Size = 12;
        mainRow.Cells[0].AddParagraph("РЕЗУЛЬТАТЫ ПРОВЕРКИ ПО ВНУТРЕННИМ БАЗАМ");
        mainRow.Cells[0].AddParagraph();

        Table tableForHeader = new Table();
        tableForHeader.Borders.Visible = true;
        tableForHeader.Borders.Color = Colors.Gray;
        tableForHeader.Format.Alignment = ParagraphAlignment.Left;
        tableForHeader.AddColumn(WidthAsPercent(100));
        table.LeftPadding = Unit.FromCentimeter(0);
        Row currentRowForHeader = tableForHeader.AddRow();
        switch (_mdv.Checked_Person)
        {
            case "Client":
                currentRowForHeader.Cells[0].AddParagraph($"Заемщик {_client.CliName}").Format.Font.Bold = true;
                break;
            case "Guarantor":
                currentRowForHeader.Cells[0].AddParagraph($"Гарант {_client.CliName}").Format.Font.Bold = true;
                break;
            case "Warrantor":
                currentRowForHeader.Cells[0].AddParagraph($"Поручитель {_client.CliName}").Format.Font.Bold = true;
                break;
            case "Pledger":
                currentRowForHeader.Cells[0].AddParagraph($"Залогодатель {_client.CliName}").Format.Font.Bold = true;
                break;
            case "Codebtor":
                currentRowForHeader.Cells[0].AddParagraph($"Созаемщик {_client.CliName}").Format.Font.Bold = true;
                break;
        }
        if (_client.CliJurFl == 1)
        {
            currentRowForHeader.Cells[0].AddParagraph($"БИН - {_client.CliBinIin}").Format.Font.Bold = false;
        }
        else
        {
            currentRowForHeader.Cells[0].AddParagraph($"ИИН - {_client.CliBinIin}").Format.Font.Bold = false;
        }

        XmlDocument xResult = new XmlDocument();
        xResult.InnerXml = _mdv._MsbDbCheck.DbcResult;
        AddTableWithTitle(currentRowForHeader, "Список лиц, не рекомендуемых к кредитованию", 0, 490);
        Table tableUnadvisory = GetNewTable(new int[] { 150, 340 });
        XmlNode CheckUnadvisory = xResult.SelectSingleNode(_mdv.Checked_Person + "/CheckUnadvisory");
        Row currentUnadvisory = tableUnadvisory.AddRow();
        currentUnadvisory.Borders.Visible = true;
        AddRowDouble(currentUnadvisory, "Статус проверки:", CheckStatus("CheckStatus", CheckUnadvisory));
        AddRowDouble(currentUnadvisory, "Результат поиска:", GetResult("Found", CheckUnadvisory));
        AddRowDouble(currentUnadvisory, "Комментарий:", "Comment");
        currentRowForHeader.Cells[0].Elements.Add(tableUnadvisory);

        /*if ((_mdv.Checked_Person.Equals("FirstChiefCheck")) || (!_mdv.Checked_Person.Equals("FirstChiefCheck") && _client.CliJurFl != 1))
        {
            Table tableUnadvisory = new Table();
            tableUnadvisory.Borders.Visible = true;
            tableUnadvisory.Borders.Color = Colors.Gray;
            tableUnadvisory.Format.Alignment = ParagraphAlignment.Left;
            XmlNode CheckUnadvisory = xResult.SelectSingleNode(_mdv.Checked_Person + "/CheckUnadvisory");
            tableUnadvisory.AddColumn(WidthAsPercent(100));
            Row currentUnadvisory = tableUnadvisory.AddRow();
            currentUnadvisory.Cells[0].AddParagraph("Список лиц, не рекомендуемых к кредитованию");
            Column column;
            column = tableUnadvisory.AddColumn(WidthAsPercent(25));
            column = tableUnadvisory.AddColumn(WidthAsPercent(75));
            AddRowDouble(currentUnadvisory, "Статус проверки:", CheckStatus("CheckStatus", CheckUnadvisory));
            AddRowDouble(currentUnadvisory, "Результат поиска:", GetResult("Found",CheckUnadvisory));
            AddRowDouble(currentUnadvisory, "Комментарий:", CheckUnadvisory.SelectSingleNode("Comment").InnerText);
            currentRowForHeader.Cells[0].Elements.Add(tableUnadvisory);
        }*/

        mainRow.Cells[0].Elements.Add(tableForHeader);
        mainRow.Cells[0].AddParagraph();


        /*XmlNode Accounts = xResult.SelectSingleNode(_mdv.Checked_Person + "/Accounts");
        AddSection(table1, "Информация по текущим счетам клиента");
        AddSection(table1, "ИНФОРМАЦИЯ В РОЗНИЧНОЙ АБС");
        AddRow(table1, "Статус проверки:", CheckStatus("StatusRS/CheckStatus", Accounts));
        AddRow(table1, "Результат поиска:", GetResult("StatusRS/Found", Accounts));
        if (Accounts.SelectSingleNode("StatusRS/Found").InnerText.Equals("true") && Accounts.SelectNodes("Record") != null)
        {
            XmlNodeList AccountList = Accounts.SelectNodes("Record");
            int i = 0;
            if (AccountList.Count > 0)
            {
                AddSection(table1, "Список текущих счетов:");
                foreach (XmlNode AccXn in AccountList) 
                {
                    if (AccXn.SelectSingleNode("ACCTYPE").InnerText.Equals("Текущие счета") && AccXn.SelectSingleNode("SYSTEMTYPE").InnerText.Equals("Колвир розница")) 
                    {
                        i++;
                    }
                }
            }
        }
        AddRow(table1, "Комментарий:", Accounts.SelectSingleNode("Comment").InnerText);*/

    }
    private string CheckStatus(string node, XmlNode xmlNode)
    {
        return "Не проверено";
        if (xmlNode.SelectSingleNode(node).InnerText.Equals("Unchecked"))
        {
            return "Не проверено";
        }
        else if (xmlNode.SelectSingleNode("CheckStatus").InnerText.Equals("Checked"))
        {
            return "Проверено";
        }
        else
        {
            return "Ошибка";
        }
    }

    private string GetResult(string node, XmlNode xmlNode)
    {
        return "Найден";
        if (xmlNode.SelectSingleNode(node).InnerText.Equals("true"))
        {
            return "Найден";
        }
        else
        {
            return "Не найден";
        }
    }

    private Table GetNewTable(int[] columns) 
    {
        Table table = new Table();
        table.Format.Font.Name = "Times New Roman";
        table.Borders.Visible = true;
        table.Borders.Color = Colors.Gray;
        table.Format.Alignment = ParagraphAlignment.Left;
        table.Format.SpaceAfter = 0;
        table.Format.SpaceBefore = 0;
        Column column;
        foreach (var i in columns) 
        {
            column = table.AddColumn(i);
        }
        return table;
    }

    private void AddTableWithTitle(Row row, string title, int index, int size)
    {
        Table titleTable = GetNewTable(new int[] { size });
        Row titleRow = titleTable.AddRow();
        titleRow.Cells[0].AddParagraph(title);
        row.Cells[index].Elements.Add(titleTable);
    }

    private void AddRowDouble(Row row, string first, string second) 
    {
        row.Borders.Visible = true;
        row.Cells[0].AddParagraph(first);
        row.Cells[1].AddParagraph(second);
    }
