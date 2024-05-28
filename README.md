    public override void DrawDocument()
    {
        Table tableForHeader = CurrentSection.AddTable();
        tableForHeader.Format.Alignment = ParagraphAlignment.Right;
        tableForHeader.AddColumn(WidthAsPercent(100));
        tableForHeader.Format.Font.Bold = true;
        tableForHeader.LeftPadding = Unit.FromCentimeter(0);
        Row currentRowForHeader = tableForHeader.AddRow();
        string date = DateTime.Now.ToString("HH:mm:ss - dd.MM.yyyy года");
        currentRowForHeader.Cells[0].AddParagraph($"Дата и время запроса {date}");
        currentRowForHeader.Cells[0].AddParagraph();
        currentRowForHeader = tableForHeader.AddRow();
        currentRowForHeader.Format.Font.Bold = true;
        currentRowForHeader.Cells[0].Format.Alignment = ParagraphAlignment.Center;
        currentRowForHeader.Cells[0].Format.Font.Size = 12;
        currentRowForHeader.Cells[0].AddParagraph("РЕЗУЛЬТАТЫ ПРОВЕРКИ ПО ВНУТРЕННИМ БАЗАМ");
        currentRowForHeader.Cells[0].AddParagraph();

        Table mainTable = CurrentSection.AddTable();
        mainTable.Borders.Visible = true;
        mainTable.Borders.Color = Colors.Gray;
        mainTable.Format.Alignment = ParagraphAlignment.Left;
        mainTable.AddColumn(WidthAsPercent(100));
        Row mainRow = mainTable.AddRow();
        switch (_mdv.Checked_Person)
        {
            case "ClientCheck":
                mainRow.Cells[0].AddParagraph($"Заемщик {_client.CliName}").Format.Font.Bold = true;
                break;
            case "GuarantorCheck":
                mainRow.Cells[0].AddParagraph($"Гарант {_client.CliName}").Format.Font.Bold = true;
                break;
            case "WarrantorCheck":
                mainRow.Cells[0].AddParagraph($"Поручитель {_client.CliName}").Format.Font.Bold = true;
                break;
            case "PledgerCheck":
                mainRow.Cells[0].AddParagraph($"Залогодатель {_client.CliName}").Format.Font.Bold = true;
                break;
            case "CodebtorCheck":
                mainRow.Cells[0].AddParagraph($"Созаемщик {_client.CliName}").Format.Font.Bold = true;
                break;
        }
        if (_client.CliJurFl == 1)
        {
            mainRow.Cells[0].AddParagraph($"БИН - {_client.CliBinIin}").Format.Font.Bold = false;
        }
        else
        {
            mainRow.Cells[0].AddParagraph($"ИИН - {_client.CliBinIin}").Format.Font.Bold = false;
        }
        mainRow.Cells[0].AddParagraph();

        XmlDocument xResult = new XmlDocument();
        xResult.InnerXml = _mdv._MsbDbCheck.DbcResult;

        if ((_mdv.Checked_Person.Equals("FirstChiefCheck")) || (!_mdv.Checked_Person.Equals("FirstChiefCheck") && _client.CliJurFl != 1))
        {
            AddTableWithTitle(mainRow, "Список лиц, не рекомендуемых к кредитованию", 0, 490);
            Table tableUnadvisory = GetNewTable(new int[] { 100, 390 }, 10);
            XmlNode CheckUnadvisory = xResult.SelectSingleNode(_mdv.Checked_Person + "/CheckUnadvisory");
            Row currentUnadvisory = tableUnadvisory.AddRow();
            AddRow(tableUnadvisory, currentUnadvisory, new List<string>() { "Статус проверки:", CheckStatus("CheckStatus", CheckUnadvisory) }, true, 4);
            AddRow(tableUnadvisory, currentUnadvisory, new List<string>() { "Результат поиска:", GetResult("Found", CheckUnadvisory) }, false, 4);
            AddRow(tableUnadvisory, currentUnadvisory, new List<string>() { "Комментарий:", CheckUnadvisory.SelectSingleNode("Comment").InnerText }, false, 4);
            mainRow.Cells[0].Elements.Add(tableUnadvisory);
            mainRow.Cells[0].AddParagraph();
        }

        AddTableWithTitle(mainRow, "Информация по текущим счетам клиента", 0, 490);
        AddTableWithTitle(mainRow, "ИНФОРМАЦИЯ В РОЗНИЧНОЙ АБС", 0, 490);
        XmlNode Accounts = xResult.SelectSingleNode(_mdv.Checked_Person + "/Accounts");
        Table tableAccounts = GetNewTable(new int[] { 100, 390 }, 10);
        tableAccounts.LeftPadding = 0;
        Row currentAccounts = tableAccounts.AddRow();
        XmlNodeList AccountList = Accounts.SelectNodes("Record");
        AddRow(tableAccounts, currentAccounts, new List<string>() { "Статус проверки:", CheckStatus("StatusRS/CheckStatus", Accounts) }, true, 4);
        AddRow(tableAccounts, currentAccounts, new List<string>() { "Результат поиска:", GetResult("StatusRS/Found", Accounts) }, false, 4);
        if (Accounts.SelectSingleNode("StatusRS/Found").InnerText.Equals("true") && Accounts.SelectNodes("Record") != null)
        {
            int i = 0;
            if (AccountList.Count > 0)
            {
                currentAccounts = tableAccounts.AddRow();
                currentAccounts.Cells[0].AddParagraph("Список текущих счетов:");
                currentAccounts.Cells[0].Format.LeftIndent = 4;
                Table accountInfoTable;
                Row accountInfoRow;
                foreach (XmlNode AccXn in AccountList)
                {
                    if (AccXn.SelectSingleNode("ACCTYPE").InnerText.Equals("Текущие счета") && AccXn.SelectSingleNode("SYSTEMTYPE").InnerText.Equals("Колвир розница"))
                    {
                        i++;
                        AddTableWithTitle(currentAccounts, $"{i} Текущий счет", 1, 390);
                        accountInfoTable = GetNewTable(new int[] { 80, 120, 90, 100 }, 10);
                        accountInfoRow = accountInfoTable.AddRow();
                        AddRow(accountInfoTable, accountInfoRow, new List<string>() { "Номер счета:", AccXn.SelectSingleNode("ACCNUM").InnerText, "Баланс:", AccXn.SelectSingleNode("BALANCE").InnerText }, true, 0);
                        AddRow(accountInfoTable, accountInfoRow, new List<string>() { "Дата начала:", AccXn.SelectSingleNode("FROMDATE").InnerText, "Дата завершения:", AccXn.SelectSingleNode("TODATE").InnerText.Equals("31.12.4712") || @AccXn.SelectSingleNode("TODATE").InnerText.Equals("01.01.2030") ? "-" : AccXn.SelectSingleNode("TODATE").InnerText }, false, 0);
                        AddRow(accountInfoTable, accountInfoRow, new List<string>() { "Валюта:", AccXn.SelectSingleNode("CURRENCY").InnerText, "Статус:", AccXn.SelectSingleNode("STATUS").InnerText }, false, 0);
                        currentAccounts.Cells[1].Elements.Add(accountInfoTable);
                    }
                }
                i = 0;
                currentAccounts = tableAccounts.AddRow();
                currentAccounts.Cells[0].AddParagraph("Список депозитов:");
                currentAccounts.Cells[0].Format.LeftIndent = 4;
                Table depositsInfoTable;
                Row depositsInfoRow;
                foreach (XmlNode AccXn in AccountList)
                {
                    if (!AccXn.SelectSingleNode("ACCTYPE").InnerText.Equals("Текущие счета") && AccXn.SelectSingleNode("SYSTEMTYPE").InnerText.Equals("Колвир розница"))
                    {
                        i++;
                        AddTableWithTitle(currentAccounts, $"{i}  Депозит", 1, 390);
                        depositsInfoTable = GetNewTable(new int[] { 80, 120, 90, 100 }, 10);
                        depositsInfoRow = depositsInfoTable.AddRow();
                        AddRow(depositsInfoTable, depositsInfoRow, new List<string>() { "Номер счета:", AccXn.SelectSingleNode("ACCNUM").InnerText, "Баланс:", AccXn.SelectSingleNode("BALANCE").InnerText }, true, 0);
                        AddRow(depositsInfoTable, depositsInfoRow, new List<string>() { "Дата начала:", AccXn.SelectSingleNode("FROMDATE").InnerText, "Дата завершения:", AccXn.SelectSingleNode("TODATE").InnerText.Equals("31.12.4712") || @AccXn.SelectSingleNode("TODATE").InnerText.Equals("01.01.2030") ? "-" : AccXn.SelectSingleNode("TODATE").InnerText }, false, 0);
                        AddRow(depositsInfoTable, depositsInfoRow, new List<string>() { "Валюта:", AccXn.SelectSingleNode("CURRENCY").InnerText, "Статус:", AccXn.SelectSingleNode("STATUS").InnerText }, false, 0);
                        currentAccounts.Cells[1].Elements.Add(depositsInfoTable);
                    }
                }
                currentAccounts = tableAccounts.AddRow();
            }
        }
        AddRow(tableAccounts, currentAccounts, new List<string>() { "Комментарий:", Accounts.SelectSingleNode("StatusRS").InnerText }, false, 4);
        mainRow.Cells[0].Elements.Add(tableAccounts);

        Table tableAccounts2 = GetNewTable(new int[] { 100, 390 }, 10);
        Row currentAccounts2 = tableAccounts2.AddRow();
        tableAccounts2.LeftPadding = 0;
        AddTableWithTitle(mainRow, "ИНФОРМАЦИЯ В КОРПОРАТИВНОЙ АБС", 0, 490);
        AddRow(tableAccounts2, currentAccounts2, new List<string>() { "Статус проверки:", CheckStatus("StatusBS/CheckStatus", Accounts) }, true, 4);
        AddRow(tableAccounts2, currentAccounts2, new List<string>() { "Результат поиска:", GetResult("StatusBS/Found", Accounts) }, false, 4);
        if (Accounts.SelectSingleNode("StatusBS/Found").InnerText.Equals("true") && Accounts.SelectNodes("Record") != null)
        {
            int i = 0;
            if (AccountList.Count > 0)
            {
                currentAccounts2 = tableAccounts2.AddRow();
                currentAccounts2.Cells[0].AddParagraph("Список текущих счетов:");
                currentAccounts2.Cells[0].Format.LeftIndent = 4;
                Table accountInfoTable;
                Row accountInfoRow;
                foreach (XmlNode AccXn in AccountList)
                {
                    if (AccXn.SelectSingleNode("ACCTYPE").InnerText.Equals("Текущие счета") && !AccXn.SelectSingleNode("SYSTEMTYPE").InnerText.Equals("Колвир розница"))
                    {
                        i++;
                        AddTableWithTitle(currentAccounts2, $"{i} Текущий счет", 1, 390);
                        accountInfoTable = GetNewTable(new int[] { 80, 120, 90, 100 }, 10);
                        accountInfoRow = accountInfoTable.AddRow();
                        AddRow(accountInfoTable, accountInfoRow, new List<string>() { "Номер счета:", AccXn.SelectSingleNode("ACCNUM").InnerText, "Баланс:", AccXn.SelectSingleNode("BALANCE").InnerText }, true, 0);
                        AddRow(accountInfoTable, accountInfoRow, new List<string>() { "Дата начала:", AccXn.SelectSingleNode("FROMDATE").InnerText, "Дата завершения:", AccXn.SelectSingleNode("TODATE").InnerText.Equals("31.12.4712") || @AccXn.SelectSingleNode("TODATE").InnerText.Equals("01.01.2030") ? "-" : AccXn.SelectSingleNode("TODATE").InnerText }, false, 0);
                        AddRow(accountInfoTable, accountInfoRow, new List<string>() { "Валюта:", AccXn.SelectSingleNode("CURRENCY").InnerText, "Статус:", AccXn.SelectSingleNode("STATUS").InnerText }, false, 0);
                        currentAccounts2.Cells[1].Elements.Add(accountInfoTable);
                    }
                }
                i = 0;
                currentAccounts2 = tableAccounts2.AddRow();
                currentAccounts2.Cells[0].AddParagraph("Список депозитов:");
                currentAccounts2.Cells[0].Format.LeftIndent = 4;
                Table depositsInfoTable;
                Row depositsInfoRow;
                foreach (XmlNode AccXn in AccountList)
                {
                    if (!AccXn.SelectSingleNode("ACCTYPE").InnerText.Equals("Текущие счета") && !AccXn.SelectSingleNode("SYSTEMTYPE").InnerText.Equals("Колвир розница"))
                    {
                        i++;
                        AddTableWithTitle(currentAccounts2, $"{i}  Депозит", 1, 390);
                        depositsInfoTable = GetNewTable(new int[] { 80, 120, 90, 100 }, 10);
                        depositsInfoRow = depositsInfoTable.AddRow();
                        AddRow(depositsInfoTable, depositsInfoRow, new List<string>() { "Номер счета:", AccXn.SelectSingleNode("ACCNUM").InnerText, "Баланс:", AccXn.SelectSingleNode("BALANCE").InnerText }, true, 0);
                        AddRow(depositsInfoTable, depositsInfoRow, new List<string>() { "Дата начала:", AccXn.SelectSingleNode("FROMDATE").InnerText, "Дата завершения:", AccXn.SelectSingleNode("TODATE").InnerText.Equals("31.12.4712") || @AccXn.SelectSingleNode("TODATE").InnerText.Equals("01.01.2030") ? "-" : AccXn.SelectSingleNode("TODATE").InnerText }, false, 0);
                        AddRow(depositsInfoTable, depositsInfoRow, new List<string>() { "Валюта:", AccXn.SelectSingleNode("CURRENCY").InnerText, "Статус:", AccXn.SelectSingleNode("STATUS").InnerText }, false, 0);
                        currentAccounts2.Cells[1].Elements.Add(depositsInfoTable);
                    }
                }
            }
        }
        AddRow(tableAccounts2, currentAccounts2, new List<string>() { "Комментарий:", Accounts.SelectSingleNode("StatusBS").InnerText }, false, 4);
        mainRow.Cells[0].Elements.Add(tableAccounts2);
        mainRow.Cells[0].AddParagraph();

        AddTableWithTitle(mainRow, "Информация по кредитной истории клиента", 0, 490);
        XmlNode ClientCredits = xResult.SelectSingleNode(_mdv.Checked_Person + "/ClientCredits");
        Table tableClientCredits = GetNewTable(new int[] { 100, 390 }, 10);
        tableClientCredits.LeftPadding = 0;
        Row currentClientCredits = tableClientCredits.AddRow();
        AddRow(tableClientCredits, currentClientCredits, new List<string>() { "Статус проверки:", CheckStatus("CheckStatus", ClientCredits) }, true, 4);
        currentClientCredits = tableClientCredits.AddRow();
        currentClientCredits.Cells[0].AddParagraph("Результат проверки:");
        currentClientCredits.Cells[0].Format.LeftIndent = 4;
        Table clientCreditsInfoTable = GetNewTable(new int[] { 200, 190 }, 10);
        Row cientCreditsInfoRow = clientCreditsInfoTable.AddRow();
        AddRow(clientCreditsInfoTable, cientCreditsInfoRow, new List<string>() { "Общая сумма займа (KZT):", CheckStatus("TotalInfo/TotalCreditAmountKZT", ClientCredits) }, true, 0);
        AddRow(clientCreditsInfoTable, cientCreditsInfoRow, new List<string>() { "Общ. сумма ежемесячных платежей (KZT):", CheckStatus("TotalInfo/TotalPaymentAmountKZT", ClientCredits) }, false, 0);
        AddRow(clientCreditsInfoTable, cientCreditsInfoRow, new List<string>() { "Общая сумма остатков ОД (KZT):", CheckStatus("TotalInfo/TotalMainDebtAmountKZT", ClientCredits) }, false, 0);
        AddRow(clientCreditsInfoTable, cientCreditsInfoRow, new List<string>() { "Общее кол-во допущенных просрочек:", CheckStatus("TotalInfo/TotalOverdueCount", ClientCredits) }, false, 0);
        AddRow(clientCreditsInfoTable, cientCreditsInfoRow, new List<string>() { "Просроченная задолженность (KZT):", CheckStatus("TotalInfo/CurrentTotalOverdueDebtAmountKZT", ClientCredits) }, false, 0);
        AddRow(clientCreditsInfoTable, cientCreditsInfoRow, new List<string>() { "Максимальное кол-во дней просрочек:", CheckStatus("TotalInfo/MaxOverdueDays", ClientCredits) }, false, 0);
        currentClientCredits.Cells[1].Elements.Add(clientCreditsInfoTable);
        mainRow.Cells[0].Elements.Add(tableClientCredits);

        Table tableClientCredits2 = GetNewTable(new int[] { 100, 390 }, 10);
        tableClientCredits2.LeftPadding = 0;
        Row currentClientCredits2 = tableClientCredits2.AddRow();
        AddTableWithTitle(mainRow, "ИНФОРМАЦИЯ О ЗАЙМАХ В РОЗНИЧНОЙ АБС", 0, 490);
        currentClientCredits2.Cells[0].AddParagraph("Результат проверки:");
        currentClientCredits2.Cells[0].Format.LeftIndent = 4;
        Table clientCreditsInfoTable2 = GetNewTable(new int[] { 200, 190 }, 10);
        Row cientCreditsInfoRow2 = clientCreditsInfoTable2.AddRow();
        AddRow(clientCreditsInfoTable2, cientCreditsInfoRow2, new List<string>() { "Общая сумма займа (KZT):", CheckStatus("ColvirRSTotalInfo/TotalCreditAmountKZT", ClientCredits) }, true, 0);
        AddRow(clientCreditsInfoTable2, cientCreditsInfoRow2, new List<string>() { "Общ. сумма ежемесячных платежей (KZT):", CheckStatus("ColvirRSTotalInfo/TotalPaymentAmountKZT", ClientCredits) }, false, 0);
        AddRow(clientCreditsInfoTable2, cientCreditsInfoRow2, new List<string>() { "Общая сумма остатков ОД (KZT):", CheckStatus("ColvirRSTotalInfo/TotalMainDebtAmountKZT", ClientCredits) }, false, 0);
        AddRow(clientCreditsInfoTable2, cientCreditsInfoRow2, new List<string>() { "Общее кол-во допущенных просрочек:", CheckStatus("ColvirRSTotalInfo/TotalOverdueCount", ClientCredits) }, false, 0);
        AddRow(clientCreditsInfoTable2, cientCreditsInfoRow2, new List<string>() { "Просроченная задолженность (KZT):", CheckStatus("ColvirRSTotalInfo/CurrentTotalOverdueDebtAmountKZT", ClientCredits) }, false, 0);
        AddRow(clientCreditsInfoTable2, cientCreditsInfoRow2, new List<string>() { "Максимальное кол-во дней просрочек:", CheckStatus("ColvirRSTotalInfo/MaxOverdueDays", ClientCredits) }, false, 0);
        currentClientCredits2.Cells[1].Elements.Add(clientCreditsInfoTable2);
        mainRow.Cells[0].Elements.Add(tableClientCredits2);

        XmlNodeList CreditsList = ClientCredits.SelectNodes("Credits/Credit");
        int k = 0;
        if (CreditsList.Count > 0)
        {
            Table tableCreditsList = GetNewTable(new int[] { 100, 390 }, 10);
            tableCreditsList.LeftPadding = 0;
            Row currentCreditsList = tableCreditsList.AddRow();
            currentCreditsList.Cells[0].AddParagraph("Список займов:");
            currentCreditsList.Cells[0].Format.LeftIndent = 4;
            Table currentCreditTable;
            Row currentCreditRow;
            foreach (XmlNode CreditXn in CreditsList)
            {
                if (CreditXn.SelectSingleNode("BASETYPE").InnerText.Equals("rs") && (CreditXn.SelectSingleNode("STAT_CODE").InnerText.Equals("FIN") || CreditXn.SelectSingleNode("STAT_CODE").InnerText.Equals("ACTUAL")))
                {
                    k++;
                    AddTableWithTitle(currentAccounts2, $"{k} Кредит", 1, 390);
                    currentCreditTable = GetNewTable(new int[] { 195, 195 }, 10);
                    currentCreditRow = currentCreditTable.AddRow();
                    AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Программа кредитования:", CreditXn.SelectSingleNode("DCL_NAME").InnerText}, true, 0);
                    AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Сумма займа:", CreditXn.SelectSingleNode("AMOUNT").InnerText}, false, 0);
                    AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Валюта:", CreditXn.SelectSingleNode("VAL_CODE").InnerText}, false, 0);
                    AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Дата получения:", CreditXn.SelectSingleNode("FROMDATE").InnerText}, false, 0);
                    AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Дата погашения:", CreditXn.SelectSingleNode("TODATE").InnerText}, false, 0);
                    AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Срок кредитования:", CreditXn.SelectSingleNode("PRD_NAME").InnerText}, false, 0);
                    if (CreditXn.SelectSingleNode("STAT_CODE").InnerText.Equals("FIN")) 
                    {
                        AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Статус", CreditXn.SelectSingleNode("STAT_NAME").InnerText }, false, 0);
                    }
                    else if (CreditXn.SelectSingleNode("STAT_CODE").InnerText.Equals("ACTUAL"))
                    {
                        AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Статус", CreditXn.SelectSingleNode("STAT_NAME").InnerText }, false, 0);
                    }
                    AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Остаток ОД в валюте договора:", CreditXn.SelectSingleNode("REMAINING_MAIN_DEBT").InnerText }, false, 0);
                    AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Годовая процентная ставка:", CreditXn.SelectSingleNode("PERCENT_RATE").InnerText }, false, 0);
                    AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Текущая просроченная задолженность:", CreditXn.SelectSingleNode("DELAYED_SUMM").InnerText }, false, 0);
                    AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Количество допущенных просрочек:", CreditXn.SelectSingleNode("OVERDUE_COUNT").InnerText }, false, 0);
                    AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Максимальное количество дней просрочек:", CreditXn.SelectSingleNode("MAX_OVERDUE_DAYS").InnerText }, false, 0);
                    AddRow(currentCreditTable, currentCreditRow, new List<string>() { "Целевое назначение:", CreditXn.SelectSingleNode("PUR_NAME").InnerText }, false, 0);
                    currentCreditsList.Cells[1].Elements.Add(currentCreditTable);
                }    
            }
            mainRow.Cells[0].Elements.Add(tableCreditsList);
        }

        AddTableWithTitle(mainRow, "ИНФОРМАЦИЯ О ЗАЙМАХ В КОРПОРАТИВНОЙ АБС", 0, 490);
        Table tableClientCredits3 = GetNewTable(new int[] { 100, 390 }, 10);
        tableClientCredits3.LeftPadding = 0;
        Row currentClientCredits3 = tableClientCredits3.AddRow();
        currentClientCredits3.Cells[0].AddParagraph("Результат проверки:");
        currentClientCredits3.Cells[0].Format.LeftIndent = 4;
        Table clientCreditsInfoTable3 = GetNewTable(new int[] { 200, 190 }, 10);
        Row cientCreditsInfoRow3 = clientCreditsInfoTable3.AddRow();
        AddRow(clientCreditsInfoTable3, cientCreditsInfoRow3, new List<string>() { "Общая сумма займа (KZT):", CheckStatus("ColvirBSTotalInfo/TotalCreditAmountKZT", ClientCredits) }, true, 0);
        AddRow(clientCreditsInfoTable3, cientCreditsInfoRow3, new List<string>() { "Общ. сумма ежемесячных платежей (KZT):", CheckStatus("ColvirBSTotalInfo/TotalPaymentAmountKZT", ClientCredits) }, false, 0);
        AddRow(clientCreditsInfoTable3, cientCreditsInfoRow3, new List<string>() { "Общая сумма остатков ОД (KZT):", CheckStatus("ColvirBSTotalInfo/TotalMainDebtAmountKZT", ClientCredits) }, false, 0);
        AddRow(clientCreditsInfoTable3, cientCreditsInfoRow3, new List<string>() { "Общее кол-во допущенных просрочек:", CheckStatus("ColvirBSTotalInfo/TotalOverdueCount", ClientCredits) }, false, 0);
        AddRow(clientCreditsInfoTable3, cientCreditsInfoRow3, new List<string>() { "Просроченная задолженность (KZT):", CheckStatus("ColvirBSTotalInfo/CurrentTotalOverdueDebtAmountKZT", ClientCredits) }, false, 0);
        AddRow(clientCreditsInfoTable3, cientCreditsInfoRow3, new List<string>() { "Максимальное кол-во дней просрочек:", CheckStatus("ColvirBSTotalInfo/MaxOverdueDays", ClientCredits) }, false, 0);
        currentClientCredits3.Cells[1].Elements.Add(clientCreditsInfoTable3);
        mainRow.Cells[0].Elements.Add(tableClientCredits3);


        k = 0;
        if (CreditsList.Count > 0)
        {
            Table tableCreditsList2 = GetNewTable(new int[] { 100, 390 }, 10);
            tableCreditsList2.LeftPadding = 0;
            Row currentCreditsList2 = tableCreditsList2.AddRow();
            currentCreditsList2.Cells[0].AddParagraph("Список займов:");
            currentCreditsList2.Cells[0].Format.LeftIndent = 4;
            Table currentCreditTable2;
            Row currentCreditRow2;
            foreach (XmlNode CreditXn in CreditsList)
            {
                if (CreditXn.SelectSingleNode("BASETYPE").InnerText.Equals("corp") && (CreditXn.SelectSingleNode("STAT_CODE").InnerText.Equals("FIN") || CreditXn.SelectSingleNode("STAT_CODE").InnerText.Equals("ACTUAL")))
                {
                    k++;
                    AddTableWithTitle(currentAccounts2, $"{k} Кредит", 1, 390);
                    currentCreditTable2 = GetNewTable(new int[] { 195, 195 }, 10);
                    currentCreditRow2 = currentCreditTable2.AddRow();
                    AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Программа кредитования:", CreditXn.SelectSingleNode("DCL_NAME").InnerText }, true, 0);
                    AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Сумма займа:", CreditXn.SelectSingleNode("AMOUNT").InnerText }, false, 0);
                    AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Валюта:", CreditXn.SelectSingleNode("VAL_CODE").InnerText }, false, 0);
                    AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Дата получения:", CreditXn.SelectSingleNode("FROMDATE").InnerText }, false, 0);
                    AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Дата погашения:", CreditXn.SelectSingleNode("TODATE").InnerText }, false, 0);
                    AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Срок кредитования:", CreditXn.SelectSingleNode("PRD_NAME").InnerText }, false, 0);
                    if (CreditXn.SelectSingleNode("STAT_CODE").InnerText.Equals("FIN"))
                    {
                        AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Статус", CreditXn.SelectSingleNode("STAT_NAME").InnerText }, false, 0);
                    }
                    else if (CreditXn.SelectSingleNode("STAT_CODE").InnerText.Equals("ACTUAL"))
                    {
                        AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Статус", CreditXn.SelectSingleNode("STAT_NAME").InnerText }, false, 0);
                    }
                    AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Остаток ОД в валюте договора:", CreditXn.SelectSingleNode("REMAINING_MAIN_DEBT").InnerText }, false, 0);
                    AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Годовая процентная ставка:", CreditXn.SelectSingleNode("PERCENT_RATE").InnerText }, false, 0);
                    AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Текущая просроченная задолженность:", CreditXn.SelectSingleNode("DELAYED_SUMM").InnerText }, false, 0);
                    AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Количество допущенных просрочек:", CreditXn.SelectSingleNode("OVERDUE_COUNT").InnerText }, false, 0);
                    AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Максимальное количество дней просрочек:", CreditXn.SelectSingleNode("MAX_OVERDUE_DAYS").InnerText }, false, 0);
                    AddRow(currentCreditTable2, currentCreditRow2, new List<string>() { "Целевое назначение:", CreditXn.SelectSingleNode("PUR_NAME").InnerText }, false, 0);
                    currentCreditsList2.Cells[1].Elements.Add(currentCreditTable2);
                }
            }
            AddRow(tableCreditsList2, currentCreditsList2, new List<string>() { "Комментарий:", ClientCredits.SelectSingleNode("Comment").InnerText }, false, 4);
            mainRow.Cells[0].Elements.Add(tableCreditsList2);
        }
        else 
        {
            AddRow(tableClientCredits3, currentClientCredits3, new List<string>() { "Комментарий:", ClientCredits.SelectSingleNode("Comment").InnerText }, false, 4);
        }

    }
    private string CheckStatus(string node, XmlNode xmlNode)
    {
        if (xmlNode.SelectSingleNode(node).InnerText.Equals("Unchecked"))
        {
            return "Не проверено";
        }
        else if (xmlNode.SelectSingleNode(node).InnerText.Equals("Checked"))
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
        if (xmlNode.SelectSingleNode(node).InnerText.Equals("true"))
        {
            return "Найден";
        }
        else
        {
            return "Не найден";
        }
    }

    private Table GetNewTable(int[] columns, int fontSize)
    {
        Table table = new Table();
        table.Format.Font.Name = "Times New Roman";
        table.Format.Font.Size = fontSize;
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
        Table titleTable = GetNewTable(new int[] { size }, 12);
        Row titleRow = titleTable.AddRow();
        titleRow.Cells[0].AddParagraph(title);
        row.Cells[index].Elements.Add(titleTable);
    }

    private void AddRow(Table table, Row row, List<string> lines, bool first, int padding)
    {
        if (!first)
        {
            row = table.AddRow();
        }
        for (int i = 0; i < lines.Count; i++)
        {
            row.Cells[i].AddParagraph(lines[i]);
            if (padding != 0)
            {
                row.Cells[i].Format.LeftIndent = padding;
            }
        }
    }

    Посмотри, с кодом проблем нет, но автопереход не работает на след страницу, почему? 
