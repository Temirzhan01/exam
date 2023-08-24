conn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO CARDCOLV.t_users (id, branchid, name, surname, patronymic, login, mail, admin, disabledate, lastdate, description, is_activ, positionid, visited, dep_code, post_code, post_name, tab_name, domain, deleted, login_old, hilfm) VALUES ((SELECT MAX(t2.ID) + 1 FROM CARDCOLV.t_users t2), :BRANCHID, '', '', '', :LOGIN, '', '', '', '', '', 1, '', '', '', '', '', :LOGIN2, 'universal', '', '', '');";  
                    cmd.Parameters.Add(":BRANCHID", OracleDbType.Varchar2, bracnhId, ParameterDirection.Input);
                    cmd.Parameters.Add(":LOGIN", OracleDbType.Int32, login, ParameterDirection.Input);
                    cmd.Parameters.Add(":LOGIN2", OracleDbType.Int32, login, ParameterDirection.Input);
                    cmd.ExecuteNonQuery();
