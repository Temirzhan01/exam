using (var connection = new OracleConnection(connectionString))
{
    connection.Open();

    using (var command = new OracleCommand("cardcolv.P_reference_ForService10.GetAutorityPersonsNew", connection))
    {
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.Add(new OracleParameter("Login_", OracleDbType.Varchar2, login.ToUpper(), ParameterDirection.Input));
        command.Parameters.Add(new OracleParameter("cur_out", OracleDbType.RefCursor, ParameterDirection.Output));

        using (var reader = command.ExecuteReader())
        {
            var result = new List<AuthorizedPerson>();

            while (reader.Read())
            {
                result.Add(new AuthorizedPerson
                {
                    username = reader.GetString(reader.GetOrdinal("username")),
                    attributname = reader.GetString(reader.GetOrdinal("attributname")),
                    attorney_date = reader.GetString(reader.GetOrdinal("attorney_date")),
                    attorney = reader.GetString(reader.GetOrdinal("attorney")),
                    login = reader.GetString(reader.GetOrdinal("login")),
                    attributid = reader.GetString(reader.GetOrdinal("attributid")),
                    branch = reader.GetString(reader.GetOrdinal("branch")),
                    branchid = reader.GetInt32(reader.GetOrdinal("branchid")),
                    authorityType = reader.GetString(reader.GetOrdinal("authorityType")),
                });
            }

            return result;
        }
    }
}
