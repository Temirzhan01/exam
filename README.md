        Table tableAccounts2 = GetNewTable(new int[] { 100, 390 }, 10);
        Row currentAccounts2 = tableAccounts2.AddRow();
        AddTableWithTitle(mainRow, "ИНФОРМАЦИЯ В КОРПОРАТИВНОЙ АБС", 0, 490);
        AddRow(tableAccounts2, currentAccounts2, new List<string>() { "Статус проверки:", CheckStatus("StatusBS/CheckStatus", Accounts) }, true);
        AddRow(tableAccounts2, currentAccounts2, new List<string>() { "Результат поиска:", GetResult("StatusBS/Found", Accounts) }, false);
        if (Accounts.SelectSingleNode("StatusBS/Found").InnerText.Equals("true") && Accounts.SelectNodes("Record") != null)
        {
            int i = 0;
            if (AccountList.Count > 0)
            {
                currentAccounts2 = tableAccounts2.AddRow();
                currentAccounts2.Cells[0].AddParagraph("Список текущих счетов:");
                Table accounInfoTable;
                Row accountInfoRow;
                foreach (XmlNode AccXn in AccountList)
                {
                    if (AccXn.SelectSingleNode("ACCTYPE").InnerText.Equals("Текущие счета") && !AccXn.SelectSingleNode("SYSTEMTYPE").InnerText.Equals("Колвир розница"))
                    {
                        i++;
                        AddTableWithTitle(currentAccounts2, $"{i} Текущий счет", 1, 390);
                        accounInfoTable = GetNewTable(new int[] { 80, 120, 90, 95 }, 10);
                        accountInfoRow = accounInfoTable.AddRow();
                        AddRow(accounInfoTable, accountInfoRow, new List<string>() { "Номер счета:", AccXn.SelectSingleNode("ACCNUM").InnerText, "Баланс:", AccXn.SelectSingleNode("BALANCE").InnerText }, true);
                        AddRow(accounInfoTable, accountInfoRow, new List<string>() { "Дата начала:", AccXn.SelectSingleNode("FROMDATE").InnerText, "Дата завершения:", AccXn.SelectSingleNode("TODATE").InnerText.Equals("31.12.4712") || @AccXn.SelectSingleNode("TODATE").InnerText.Equals("01.01.2030") ? "-" : AccXn.SelectSingleNode("TODATE").InnerText }, false);
                        AddRow(accounInfoTable, accountInfoRow, new List<string>() { "Валюта:", AccXn.SelectSingleNode("CURRENCY").InnerText, "Статус:", AccXn.SelectSingleNode("STATUS").InnerText }, false);
                        currentAccounts2.Cells[1].Elements.Add(accounInfoTable); //вот тут добавляю таблицу, но вопрос, как убрать отступ внутри cell? а то границы таблицы пробивают правый край, я бы хотел сжать таблицу влево
                    }
                }
                i = 0;
                currentAccounts2 = tableAccounts2.AddRow();
                currentAccounts2.Cells[0].AddParagraph("Список депозитов:");
                Table depositsInfoTable;
                Row depositsInfoRow;
                foreach (XmlNode AccXn in AccountList)
                {
                    if (!AccXn.SelectSingleNode("ACCTYPE").InnerText.Equals("Текущие счета") && !AccXn.SelectSingleNode("SYSTEMTYPE").InnerText.Equals("Колвир розница"))
                    {
                        i++;
                        AddTableWithTitle(currentAccounts2, $"{i}  Депозит", 1, 390);
                        depositsInfoTable = GetNewTable(new int[] { 80, 120, 90, 95 }, 10);
                        depositsInfoRow = depositsInfoTable.AddRow();
                        AddRow(depositsInfoTable, depositsInfoRow, new List<string>() { "Номер счета:", AccXn.SelectSingleNode("ACCNUM").InnerText, "Баланс:", AccXn.SelectSingleNode("BALANCE").InnerText }, true);
                        AddRow(depositsInfoTable, depositsInfoRow, new List<string>() { "Дата начала:", AccXn.SelectSingleNode("FROMDATE").InnerText, "Дата завершения:", AccXn.SelectSingleNode("TODATE").InnerText.Equals("31.12.4712") || @AccXn.SelectSingleNode("TODATE").InnerText.Equals("01.01.2030") ? "-" : AccXn.SelectSingleNode("TODATE").InnerText }, false);
                        AddRow(depositsInfoTable, depositsInfoRow, new List<string>() { "Валюта:", AccXn.SelectSingleNode("CURRENCY").InnerText, "Статус:", AccXn.SelectSingleNode("STATUS").InnerText }, false);
                        currentAccounts2.Cells[1].Elements.Add(depositsInfoTable);
                    }
                }
            }
        }
        AddRow(tableAccounts2, currentAccounts2, new List<string>() { "Комментарий:", Accounts.SelectSingleNode("StatusBS").InnerText }, false);
        mainRow.Cells[0].Elements.Add(tableAccounts2);
        mainRow.Cells[0].AddParagraph();
