            List<AutorityPerson> autority = new List<AutorityPerson>();
            Database db = DatabaseFactory.CreateDatabase("cardcolv");
            string sqlCommand = "cardcolv.P_reference_ForService10.GetAutorityPersonsNew";
            OracleCommand cmd = (OracleCommand)db.GetStoredProcCommand(sqlCommand);
            cmd.Parameters.Add("Login_", OracleDbType.Varchar2, 150).Value = login.ToUpper();
            cmd.Parameters.Add("cur_out", OracleDbType.RefCursor, 0, string.Empty).Direction = ParameterDirection.Output;
            try
            {
                using (IDataReader reader = db.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        autority.Add(new AutorityPerson
                            {
                                username = reader["username"].ToString(),
                                attributname = reader["attributname"].ToString(),
                                attorney_date = reader["attorney_date"].ToString(),
                                attorney = reader["attorney"].ToString(),
                                login = reader["login"].ToString(),
                                attributid = reader["attributid"].ToString(),
                                branch = reader["branch"].ToString(),
                                branchid = Convert.ToInt32(reader["branchid"]),
                                authorityType = reader["authorityType"].ToString()
                            });
                    }
                }
            } это до



            ниже новый 
                        var parameters = new DynamicParameters();

            var login_ = new OracleParameter("Login_", OracleDbType.Varchar2, ParameterDirection.Input).Value = login;
            var cur_out = new OracleParameter("cur_out", OracleDbType.RefCursor, ParameterDirection.Output);

            parameters.Add("Login_", login_, DbType.String, ParameterDirection.Input);
            parameters.Add("cur_out", cur_out, DbType.Object, direction: ParameterDirection.Output);

            using (var connection = _context.CreateConnection()) 
            {
                await connection.ExecuteAsync("cardcolv.P_reference_ForService10.GetAutorityPersonsNew", parameters, commandType: CommandType.StoredProcedure);
                using (var reader = parameters.Get<OracleRefCursor>("cur_out").GetDataReader()) 
                {
                    List<AuthorizedPerson> result = new List<AuthorizedPerson> ();

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
                            branchid = Convert.ToInt32(reader.GetString(reader.GetOrdinal("branchid"))),
                            authorityType = reader.GetString(reader.GetOrdinal("authorityType")),
                        });
                    }

                    return result;
                }
            }

            System.ArgumentException: Value does not fall within the expected range.

            
