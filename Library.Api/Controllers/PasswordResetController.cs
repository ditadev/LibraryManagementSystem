using Library.Api.Dtos;
using Library.Features;
using Library.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class PasswordResetController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly DataContext _dataContext;

    public PasswordResetController(DataContext dataContext, IRegistrationService registrationService)
    {
        _dataContext = dataContext;
        _registrationService = registrationService;
    }

    [HttpPost]
    public async Task<ActionResult> ForgotPassword(string Email)
    {
        var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Email == Email);
        if (customer == null) return BadRequest("User not found :(");

        customer.PasswordResetToken = await _registrationService.CreateRandomToken();
        customer.ResetTokenExpires = DateTime.UtcNow.AddMinutes(5);
        await _dataContext.SaveChangesAsync();

        return Ok("You may now reset your password :)");
    }

    [HttpPost]
    public async Task<ActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.PasswordResetToken == request.Token);
        if (customer == null || customer.ResetTokenExpires < DateTime.UtcNow) return BadRequest("Invalid Token :(");

        var passwordHash = await _registrationService.CreatePasswordHash(request.Password);
        customer.PasswordResetToken = null;
        customer.ResetTokenExpires = null;
        customer.PasswordHash = passwordHash;

        await _dataContext.SaveChangesAsync();

        return Ok("Password successfully reset :)");
    }
}