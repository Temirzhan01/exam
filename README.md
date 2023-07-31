public string AddNewField(string inSchemeName, string inFieldName)
{
    // Получите контекст базы данных из вашего DbContext
    var context = _yourDbContext; // здесь подставьте ваш экземпляр DbContext

    // Создайте объект команды для хранимой процедуры
    using (var command = context.Database.GetDbConnection().CreateCommand())
    {
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "public.add_new_field";

        // Добавьте входные параметры
        command.Parameters.Add(new NpgsqlParameter("in_scheme_name", inSchemeName));
        command.Parameters.Add(new NpgsqlParameter("in_field_name", inFieldName));

        // Добавьте выходной параметр
        var outParam = new NpgsqlParameter("out_result", DbType.String);
        outParam.Direction = ParameterDirection.Output;
        command.Parameters.Add(outParam);

        // Откройте соединение и выполните хранимую процедуру
        context.Database.OpenConnection();
        command.ExecuteNonQuery();

        // Получите выходное значение
        var result = (string)outParam.Value;

        return result;
    }
}
