using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mini_Project.Models;
using System.Runtime.CompilerServices;

namespace Mini_Project.Controllers
{
    // Define DTOs
    public class profileDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

    }

    [Authorize(Roles = "Student")]
    [Route("api/student")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public StudentController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // Student Retrieve Profile API
        // GET /api/student/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var dto = new profileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return Ok(dto);
        }


        // Student Update Profile API
        // PUT /api/student/profile
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] profileDto dto)
        {
            var user = await _userManager.GetUserAsync (User);
            if (user == null)
                return NotFound();

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "Profile updated successfully" });
        }
        /*
        // Student View Self Enrollment Application List API
        // GET api/student/enrollments
        [HttpGet("enrollments")]
        public async Task<IActionResult> GetEnrollments()
        {
            var enroll = await _userManager.GetUserAsync(Enrollment);
        }*/
    }
}
