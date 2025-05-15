using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_Project.Models;
using Mini_Project.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;

namespace Mini_Project.Controllers
{
    //Define DTOs
    public class RegisterDto
    {
        public string? Username { get; set;}
        public string? Email { get; set;}
        public string? Password { get; set;}
    }

    public class LoginDto
    {
        public string? Email { get; set;}
        public string? Password { get; set;}
    }

    public class ChangePasswordDto
    {
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Student Sign Up API
        // POST /api/auth/signup
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody] RegisterDto model)
        {
            var user = new AppUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student");
                return Ok(new { message = "Sign Up Successful" });

            }

            return BadRequest(result.Errors);
        }

        // Login API
        // POST /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);


            if (result.Succeeded)
            {
                return Ok(new { message = "Login successful" });
            }

            return Unauthorized(new { message = "Invalid login attempt"});
        }

        // Change Password API
        // POST /api/auth/change-password
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return BadRequest("User not found");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new { message = "Password changed successful" });
            }

            return BadRequest(result.Errors);
        }
    }
}
