namespace GameMipls.Net.Interface;

public interface IAccountService
{
    public Task<bool> IsAuth();

    public Task<string> GetName();

    public Task<string> CreatePasswordHash(string password);

    public string CreateHash();
}