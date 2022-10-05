using Library.Model;

namespace Library.Features;

public interface ICustomerService
{
    public Task<string?> CreatePasswordHash(string password);
    public bool VerifyPasswordHash(string password, string passwordHash);
    public Task<string?> CreateRandomToken();
    public Task<string> CreateJwtToken(Customer customer);
}