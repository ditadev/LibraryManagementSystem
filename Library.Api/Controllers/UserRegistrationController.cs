using Library.Api.Dtos;
using Library.Features;
using Library.Model;
using Library.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserRegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly DataContext _dataContext;

    public UserRegistrationController(DataContext dataContext, IRegistrationService registrationService)
    {
        _dataContext = dataContext;
        _registrationService = registrationService;
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterUserRequest request)
    {
        var customer = await _dataContext.Customers.Where(c => c.Email == request.Email).FirstOrDefaultAsync();
        if (customer != null) return BadRequest("User Already Exist");
        var passwordHash = await _registrationService.CreatePasswordHash(request.Password);
        var Customer = new Customer
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Address = request.Address,
            VerificationToken = await _registrationService.CreateRandomToken(),
            PasswordHash = passwordHash
        };
        _dataContext.Customers.Add(Customer);
        await _dataContext.SaveChangesAsync();
        return Ok(" Successfully Created :)");
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginUserRequest request)
    {
        var customer = await _dataContext.Customers.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (customer == null) return BadRequest("User not found :(");

        if (customer.VerifiedAt == null) return BadRequest("Not verified :(");

        if (!_registrationService.VerifyPasswordHash(request.Password, customer.PasswordHash))
            return BadRequest("Incorrect Username/Password :(");
        var token = await _registrationService.CreateCustomerJwt(customer);
        return Ok($"Welcome back, {customer.Email}! :)\n\n{token}");
    }

    [HttpPost]
    public async Task<ActionResult> Verify(string Token)
    {
        var customer = await _dataContext.Customers.FirstOrDefaultAsync(u => u.VerificationToken == Token);
        if (User == null) return BadRequest("User not found :(");

        customer.VerifiedAt = DateTime.UtcNow;
        await _dataContext.SaveChangesAsync();

        return Ok("User Verified :)");
    }
}