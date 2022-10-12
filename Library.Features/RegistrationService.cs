using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Library.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Library.Features;

public class RegistrationService : IRegistrationService
{
    private readonly AppSettings _appSettings;
    private readonly IConfiguration _configuration;

    public RegistrationService(AppSettings appSettings,IConfiguration configuration)
    {
        _appSettings = appSettings;
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

    public async Task<string> CreateCustomerJwt(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSecret));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var claims = new List<Claim> { new("UserId", user.UserId.ToString()),
            new("sub", user.UserId.ToString()), 
            new("role", "Default") };
        
        claims.AddRange(user.Roles.Select(role => new Claim("role", role.ToString())));

        return await Task.FromResult((new JwtSecurityTokenHandler().WriteToken(
            new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            ))));
    }
    public async Task<string?> CreateAdministratorJwt(Administrator admin)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, admin.AdminId),
            new(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken
        (
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: cred
        );
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return await Task.FromResult(jwt);
    }
}