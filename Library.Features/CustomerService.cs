using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Library.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Library.Features;

public class CustomerService : ICustomerService
{
    private readonly IConfiguration _configuration;

    public CustomerService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string?> CreatePasswordHash(string password)
    {
        return await Task.FromResult(BCrypt.Net.BCrypt.HashPassword(password));
    }

    public bool VerifyPasswordHash(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    public async Task<string?> CreateRandomToken()
    {
        return await Task.FromResult(Convert.ToHexString(RandomNumberGenerator.GetBytes(8)));
    }

    public async Task<string> CreateJwtToken(Customer customer)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, customer.Username),
            new(ClaimTypes.Role, "Customer")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken
        (
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return await Task.FromResult(jwt);
    }
}