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
        public async Task<ActionResult<User>> UpdateProfile(UpdateUserRequest request)
        {
            var user = await _dataContext.Customers.FirstOrDefaultAsync(c => c.Email == request.Email);
            if (user == null) return BadRequest("User not found :(");

            if (!_registrationService.VerifyPasswordHash(request.Password, user.PasswordHash))
                return BadRequest("Incorrect Username/Password :(");
            
            user.Firstname = request.Firstname;
            user.Lastname = request.Lastname;
            user.Address = request.Address;
            user.UserId = user.UserId;
            user.Email = user.Email;
            user.Books = user.Books;
            user.Library = user.Library;
            user.PasswordHash = user.PasswordHash;
            user.VerificationToken = user.VerificationToken;
            user.VerifiedAt = user.VerifiedAt;
            user.Password = user.Password;
            user.PasswordResetToken = user.PasswordResetToken;
            user.ResetTokenExpires = user.ResetTokenExpires;

            if (request.UserId != user.UserId)
                return BadRequest("Customer Id cannot be changed");
                _dataContext.Customers.Update(user);
                await _dataContext.SaveChangesAsync();
                return Ok(user);
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
