using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Api.Dtos;
using Library.Features;
using Library.Model;
using Library.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    // [Authorize(Roles = "User")]
    public class UserPanelController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly DataContext _dataContext;

        public UserPanelController(DataContext dataContext, IRegistrationService registrationService)
        {
            _dataContext = dataContext;
            _registrationService = registrationService;
        }
        
        [HttpPut]
        public async Task<ActionResult<Customer>> UpdateProfile(UpdateUserRequest request)
        {
            var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);
            if (customer == null) return BadRequest("User not found :(");

            if (!_registrationService.VerifyPasswordHash(request.Password, customer.PasswordHash))
                return BadRequest("Incorrect Username/Password :(");
            
            customer.Firstname = request.Firstname;
            customer.Lastname = request.Lastname;
            customer.Address = request.Address;
            customer.CustomerId = customer.CustomerId;
            customer.Email = customer.Email;
            customer.Books = customer.Books;
            customer.Library = customer.Library;
            customer.PasswordHash = customer.PasswordHash;
            customer.VerificationToken = customer.VerificationToken;
            customer.VerifiedAt = customer.VerifiedAt;
            customer.Password = customer.Password;
            customer.PasswordResetToken = customer.PasswordResetToken;
            customer.ResetTokenExpires = customer.ResetTokenExpires;

            if (request.CustomerId != customer.CustomerId)
                return BadRequest("Customer Id cannot be changed");
                _dataContext.Customers.Update(customer);
                await _dataContext.SaveChangesAsync();
                return Ok(customer);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(DeleteUserRequest request)
        {
            var customer = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);
            if (customer == null) return BadRequest("User not found :(");

            if (customer.VerifiedAt == null) return BadRequest("Not verified :(");

            if (!_registrationService.VerifyPasswordHash(request.Password, customer.PasswordHash))
                return BadRequest("Incorrect Username/Password :(");
            _dataContext.Customers.Remove(customer);
            await _dataContext.SaveChangesAsync();
            return Ok("Account Deleted! :( ");
        }
    }
}
