using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Api.Dtos;
using Library.Features;
using Library.Model;
using Library.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserRegistrationController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly ICustomerService _customerService;

        public UserRegistrationController(DataContext dataContext, ICustomerService customerService)
        {
            _dataContext = dataContext;
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterUserRequest request)
        {
            var customer = await _dataContext.Customers.Where(c => c.Email == request.Email).FirstOrDefaultAsync();
            if (customer != null)
            {
                return BadRequest("User Already Exist");
            }
            await _customerService.CreatePasswordHash(request.Password);
            var Customer = new Customer()
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Address = request.Address,
                VerificationToken = await _customerService.CreateRandomToken(),
            };
            _dataContext.Customers.Add(Customer);
            await _dataContext.SaveChangesAsync();
            return Ok(" Successfully Created");
        }
        
        [HttpPost]
        public async Task<ActionResult> Login(LoginUserRequest request)
        {
            var customer = await _dataContext.Customers.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (customer==null)
            {
                return BadRequest("User not found :(");
            }

            if (customer.VerifiedAt==null)
            {
                return BadRequest("Not verified :(");
            }

            if (!_customerService.VerifyPasswordHash(request.Password,customer.PasswordHash))
            {
                return BadRequest("Incorrect Password :(");
            }

            return Ok($"Welcome back, {customer.Email}! :)");
        }
        [HttpPost]
        public async Task<ActionResult> Verify(string Token)
        {
            var customer = await _dataContext.Customers.FirstOrDefaultAsync(u => u.VerificationToken ==Token);
            if (User==null)
            {
                return BadRequest("User not found :(");
            }

            customer.VerifiedAt = DateTime.UtcNow;
            await _dataContext.SaveChangesAsync();
            
            return Ok("User Verified :)");
        }
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string Email)
        {
            var customer = await _dataContext.Customers.FirstOrDefaultAsync(u => u.Email ==Email);
            if (User==null)
            {
                return BadRequest("User not found :(");
            }

            customer.PasswordResetToken = await _customerService.CreateRandomToken();
            customer.ResetTokenExpires = DateTime.UtcNow.AddMinutes(5);
            await _dataContext.SaveChangesAsync();
            
            return Ok("You may now reset your password :)");
        }
        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var customer = await _dataContext.Customers.FirstOrDefaultAsync(u => u.PasswordResetToken ==request.Token);
            if (User==null||customer.ResetTokenExpires<DateTime.UtcNow)
            {
                return BadRequest("Invalid Token :(");
            }

            var passwordHash = await _customerService.CreatePasswordHash(request.Password);
            customer.PasswordResetToken = null;
            customer.ResetTokenExpires = null;
            customer.PasswordHash = passwordHash;
            
            await _dataContext.SaveChangesAsync();
            
            return Ok("Password successfully reset :)");
        }
    }
}
