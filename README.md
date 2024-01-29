public class AuthorityService
{
    private readonly AuthorityRepository _repository;

    public AuthorityService(AuthorityRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Authority>> GetFilteredAuthoritiesAsync(string branchCode)
    {
        var authorities = await _repository.GetAuthoritiesAsync();

        return authorities.Where(a => 
            (branchCode == "DRKK1" || branchCode == "DRKK2" || branchCode == "DRKK3" 
             ? EF.Functions.Like(a.BranchCode, branchCode.Substring(0, 4) + "%") || a.BranchCode == "DKA" || a.BranchCode == "ZPP" 
             : a.BranchCode == branchCode)
            && DateTime.Now.Date < a.Validity.AddDays(1)
        ).ToList();
    }
}
