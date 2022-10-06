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
    public class AdministratorController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IRegistrationService _registrationService;

        public AdministratorController(IRegistrationService registrationService, DataContext dataContext)
        {
            _registrationService = registrationService;
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterAdminRequest request)
        {
            var Admin = await _dataContext.Administrators.Where(c => c.AdminId == request.AdminId)
                .FirstOrDefaultAsync();
            var idCheck = await _dataContext.AdminIds.Where(i => i.NewAdminId == request.AdminId)
                .FirstOrDefaultAsync();
            if (Admin != null) return BadRequest("User Already Exist :(");
            if (idCheck==null)return BadRequest("Invalid AdminId :( ");
            var passwordHash = await _registrationService.CreatePasswordHash(request.Password);
            var administrator = new Administrator()
            {
                AdminId = request.AdminId,
                Password = request.Password,
                FirstName = request.Firstname,
                LastName = request.Lastname,
                PasswordHash = passwordHash
            };
            _dataContext.Administrators.Add(administrator);
            await _dataContext.SaveChangesAsync();
            return Ok(" Successfully Created :)");
        }

        [HttpPost]
            public async Task<ActionResult> Login(LoginAdminRequest request)
            {
                var Admin = await _dataContext.Administrators.FirstOrDefaultAsync(u => u.AdminId == request.AdminId);
                if (Admin == null) return BadRequest("User not found :(");

                if (!_registrationService.VerifyPasswordHash(request.Password, Admin.PasswordHash))
                    return BadRequest("Incorrect Username/Password :(");
                var token = await _registrationService.CreateAdministratorJwt(Admin);
                return Ok($"Welcome back, {Admin.AdminId}! :)\n\n{token}");
            }

            [HttpPost]
            public async Task<ActionResult> ForgotPassword(string AdminId)
            {
                var Admin = await _dataContext.Administrators.FirstOrDefaultAsync(c => c.AdminId == AdminId);
                if (Admin == null) return BadRequest("User not found :(");

                Admin.PasswordResetToken = await _registrationService.CreateRandomToken();
                Admin.ResetTokenExpires = DateTime.UtcNow.AddMinutes(5);
                await _dataContext.SaveChangesAsync();

                return Ok("You may now reset your password :)");
            }
            
            [HttpPost]
            public async Task<ActionResult> ResetPassword(ResetPasswordRequest request)
            {
                var Admin = await _dataContext.Administrators.FirstOrDefaultAsync(c => c.PasswordResetToken == request.Token);
                if (Admin == null || Admin.ResetTokenExpires < DateTime.UtcNow) return BadRequest("Invalid Token :(");

                var passwordHash = await _registrationService.CreatePasswordHash(request.Password);
                Admin.PasswordResetToken = null;
                Admin.ResetTokenExpires = null;
                Admin.PasswordHash = passwordHash;

                await _dataContext.SaveChangesAsync();

                return Ok("Password successfully reset :)");
            }
        }
    }


