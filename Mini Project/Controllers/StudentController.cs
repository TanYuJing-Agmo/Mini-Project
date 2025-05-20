using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_Project.Data;
using Mini_Project.Dtos;
using Mini_Project.Models;
using Mini_Project.Services;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Mini_Project.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("api/student")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEnrollmentServices _enrollmentServices;
        private readonly AppDbContext _context;

        public StudentController(UserManager<AppUser> userManager, IEnrollmentServices enrollmentServices, AppDbContext context)
        {
            _userManager = userManager;
            _enrollmentServices = enrollmentServices;
            _context = context;
        }

        // Student Retrieve Profile API
        // GET /api/student/get-profile
        [HttpGet("get-profile")]
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
        // PUT /api/student/update-profile
        [HttpPut("update-profile")]
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
        // GET api/student/get-enrollments
        [HttpGet("get-enrollments")]
        public async Task<IActionResult> GetEnrollments()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var enrollments = await _enrollmentServices.GetSelfEnrollmentAsync(user.Id);

            var dtoList = await (from e in _context.Enrollments
                                 join c in _context.Courses on e.CourseId equals c.CourseId
                                 where e.StudentId == user.Id
                                 select new EnrollmentDto
                                 {
                                     EnrollmentId = e.EnrollmentId,
                                     CourseName = c.Name,
                                     Status = e.Status,
                                     EnrolledDate = e.EnrolledDate
                                 }).ToListAsync();

            return Ok(dtoList);
        }
    }
}
