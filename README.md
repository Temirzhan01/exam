        public async Task<string> GetBranchAndFillialInfoAsync(string login)
        {
            var query = "SELECT l.ow_fi FROM t_users t, t_branch tb, l_filial l WHERE UPPER(t.login) = UPPER(':LOGIN') AND tb.id = (SELECT tb.id FROM t_branch tb WHERE tb.parentid='267' START WITH tb.id = t.branchid CONNECT BY PRIOR tb.parentid = tb.id) AND l.id = tb.id";

            using (var connection = _context.CreateConnection()) 
            {
                return await connection.QueryFirstAsync<string>(query, new { LOGIN = login });  //посмотри, я пытаюсь выполнить селект который возвращает одно значение, правильно ли для оракл 11? через даппер
            }
        }
