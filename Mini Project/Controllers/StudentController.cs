using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mini_Project.Models;
using Mini_Project.Services;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Mini_Project.Dtos;

namespace Mini_Project.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("api/student")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEnrollmentServices _enrollmentServices;

        public StudentController(UserManager<AppUser> userManager, IEnrollmentServices enrollmentServices)
        {
            _userManager = userManager;
            _enrollmentServices = enrollmentServices;
        }

        // Student Retrieve Profile API
        // GET /api/student/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var dto = new ProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.UserName,
                Email = user.Email
            };

            return Ok(dto);
        }

        // Student Update Profile API
        // PUT /api/student/profile
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
        
        // Student View Self Enrollment Application List API
        // GET api/student/enrollments
        [HttpGet("enrollments")]
        public async Task<IActionResult> GetEnrollments()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var enrollments = await _enrollmentServices.GetSelfEnrollmentAsync(user.Id);

            var dtoList = enrollments.Select(e => new EnrollmentDto
            {
                EnrollmentId = e.EnrollmentId,
                CourseName = e.Course.Name,
                Status = e.Status,
                EnrolledDate = e.EnrolledDate
            }).ToList();

            return Ok(dtoList);

        }

        // PUT /api/student/change-password
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Password changed successfully" });
        }
    }
}
