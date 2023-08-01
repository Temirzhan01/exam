        [HttpPost]
        [Route("test")]
        public async Task<ActionResult<AddOrUpdateResult>> test(string Scheme_name, string Process_name)
        {
            var result = new AddOrUpdateResult()
            {
                isSuccess = true,
                errorMsg = ""
            };
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "add_process";

                    // Добавьте входные параметры
                    command.Parameters.Add(new NpgsqlParameter("in_scheme_name", Scheme_name));
                    command.Parameters.Add(new NpgsqlParameter("in_process_name", Process_name));

                    // Добавьте выходной параметр
                    var outParam = new NpgsqlParameter("out_result", DbType.String);
                    outParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outParam);

                    // Откройте соединение и выполните хранимую процедуру
                    _context.Database.OpenConnection();
                    command.ExecuteNonQuery();

                    // Получите выходное значение
                    string updateResult = command.Parameters["out_result"].Value.ToString();
                    if (updateResult != "Ok")
                    {
                        result.isSuccess = false;
                        result.errorMsg = updateResult;
                    }
                }
            }
            catch (Exception e)
            {
                result.isSuccess = false;
                result.errorMsg = e.ToString();
            }

            return result;
        }
        {
  "isSuccess": false,
  "errorMsg": "Npgsql.PostgresException (0x80004005): 42809: add_process(in_scheme_name => text, in_process_name => text) — процедура\r\n   at Npgsql.NpgsqlConnector.<>c__DisplayClass160_0.<<DoReadMessage>g__ReadMessageLong|0>d.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at Npgsql.NpgsqlConnector.<>c__DisplayClass160_0.<<DoReadMessage>g__ReadMessageLong|0>d.MoveNext()\r\n--- End of stack trace from previous location where exception was thrown ---\r\n   at Npgsql.NpgsqlDataReader.NextResult(Boolean async, Boolean isConsuming)\r\n   at Npgsql.NpgsqlDataReader.NextResult()\r\n   at Npgsql.NpgsqlCommand.ExecuteReaderAsync(CommandBehavior behavior, Boolean async, CancellationToken cancellationToken)\r\n   at Npgsql.NpgsqlCommand.ExecuteNonQuery(Boolean async, CancellationToken cancellationToken)\r\n   at Npgsql.NpgsqlCommand.ExecuteNonQuery()\r\n   at AutoTestDataService.Controllers.ProcessController.test(String Scheme_name, String Process_name) in D:\\repos\\BitBucket\\autotestdataservice\\AutoTestDataService\\Controllers\\ProcessController.cs:line 86\r\n  Exception data:\r\n    Severity: ОШИБКА\r\n    SqlState: 42809\r\n    MessageText: add_process(in_scheme_name => text, in_process_name => text) — процедура\r\n    Hint: Для вызова процедуры используйте CALL.\r\n    Position: 15\r\n    File: parse_func.c\r\n    Line: 302\r\n    Routine: ParseFuncOrColumn"
