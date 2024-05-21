MigraDoc.DocumentObjectModel Как этот инструмент работает?

    public override void DrawDocument()
    {
        Table table = CurrentSection.AddTable();
        table.Format.Alignment = ParagraphAlignment.Right;
        table.AddColumn(WidthAsPercent(100));
        table.Format.Font.Bold = true;
        table.LeftPadding = "0cm";
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
        if (_mdv.Checked_Person == "Client")
        {
            currentRow1.Cells[0].AddParagraph($"Заемщик {_client.CliName}").Format.Font.Bold = true;
        }
        else if (_mdv.Checked_Person == "Guarantor")
        {
            currentRow1.Cells[0].AddParagraph($"Гарант {_client.CliName}").Format.Font.Bold = true;
        }
        else if (_mdv.Checked_Person == "Warrantor")
        {
            currentRow1.Cells[0].AddParagraph($"Поручитель {_client.CliName}").Format.Font.Bold = true;
        }
        else if (_mdv.Checked_Person == "Pledger")
        {
            currentRow1.Cells[0].AddParagraph($"Залогодатель {_client.CliName}").Format.Font.Bold = true;
        }
        else if (_mdv.Checked_Person == "Codebtor")
        {
            currentRow1.Cells[0].AddParagraph($"Созаемщик {_client.CliName}").Format.Font.Bold = true;
        }
        if (_client.CliJurFl == 1)
        {
            currentRow1.Cells[0].AddParagraph($"БИН - {_client.CliBinIin}").Format.Font.Bold = false;
        }
        else
        {
            currentRow1.Cells[0].AddParagraph($"ИИН - {_client.CliBinIin}").Format.Font.Bold = false;
        }
        нужно тут продолжить, чтобы формировать дальше документ который я отправлял
    }


            public static string InternalDbCheckDoc(BaseViewModel allRates, string cliId, int index)
        {
            try
            {
                InternalDbCheckBuilder DocBuilder = new InternalDbCheckBuilder(allRates, cliId, index);

                Publisher publisher = new Publisher();
                publisher.UseDefaultCredentials = true;
                Carrier carrier = new Carrier()
                {
                    _Guid = DocBuilder.GetGuid(),
                    _MdDDL = DocBuilder.GetMdDDL()
                };
                var url = publisher.Publish(carrier).URL;
                Thread.Sleep(1000);
                return url;
            }
            catch (Exception ex)
            {
                throw new Exception("InternalDbCheckDoc error", ex);
            }
        }

        
