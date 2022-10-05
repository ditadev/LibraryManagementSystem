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
    private readonly ICustomerService _customerService;
    private readonly DataContext _dataContext;

    public PasswordResetController(DataContext dataContext, ICustomerService customerService)
    {
        _dataContext = dataContext;
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<ActionResult> ForgotPassword(string Email)
    {
        var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Email == Email);
        if (customer == null) return BadRequest("User not found :(");

        customer.PasswordResetToken = await _customerService.CreateRandomToken();
        customer.ResetTokenExpires = DateTime.UtcNow.AddMinutes(5);
        await _dataContext.SaveChangesAsync();

        return Ok("You may now reset your password :)");
    }

    [HttpPost]
    public async Task<ActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.PasswordResetToken == request.Token);
        if (customer == null || customer.ResetTokenExpires < DateTime.UtcNow) return BadRequest("Invalid Token :(");

        var passwordHash = await _customerService.CreatePasswordHash(request.Password);
        customer.PasswordResetToken = null;
        customer.ResetTokenExpires = null;
        customer.PasswordHash = passwordHash;

        await _dataContext.SaveChangesAsync();

        return Ok("Password successfully reset :)");
    }
}