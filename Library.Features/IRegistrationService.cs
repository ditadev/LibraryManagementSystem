using Library.Model;

namespace Library.Features;

public interface IRegistrationService
{
    public Task<string?> CreatePasswordHash(string password);
    public bool VerifyPasswordHash(string password, string passwordHash);
    public Task<string?> CreateRandomToken();
    public Task<string> CreateCustomerJwt(User user);
    public Task<string> CreateAdministratorJwt(Administrator admin);
}