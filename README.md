        public async Task<IEnumerable<Authority>> GetAuthoritiesAsync(string branchNumber)
        {
            var query = _context.AUTHORITIES_MSB;
            if (branchNumber == "DRKK1" || branchNumber == "DRKK2" || branchNumber == "DRKK3")
            {
                var likeCode = branchNumber.Substring(0, 4);
                return query.Where(a => EF.Functions.Like(a.BRANCH_CODE, likeCode) || a.BRANCH_CODE == "DKA" || a.BRANCH_CODE == "ZPP" && DateTime.Now.Date < a.VALIDITY.AddDays(1)).ToList();
            }
            else
            {
                return query.Where(a => a.BRANCH_CODE == branchNumber && DateTime.Now.Date < a.VALIDITY.AddDays(1)).ToList();
            }
        }
        {
    "error": "ORA-00604: error occurred at recursive SQL level 1\nORA-01882: timezone region not found"
      }
