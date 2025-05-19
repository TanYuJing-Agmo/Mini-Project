using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mini_Project.Models;
using Mini_Project.Services;
using Mini_Project.Dtos;

namespace Mini_Project.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEnrollmentServices _enrollmentServices;
        private readonly IConfiguration _configuration;

        public AdminController(IEnrollmentServices enrollmentServices, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _enrollmentServices = enrollmentServices;
            _userManager = userManager;
            _configuration = configuration;
        }

        //CRUD Admin
        // GET /api/admin/get-admin
        [HttpGet("get-admin")]
        public async Task<IActionResult> GetAdmin()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            if (admins == null)
            {
                return NotFound();
            }

            var dto = admins.Select(user => new AdminDto { Username = user.UserName }).ToList();

            return Ok(dto);
        }

        // POST /api/admin/add-admin
        [HttpPost("add-admin")]
        public async Task<IActionResult> AddAdmin([FromBody] AdminDto dto)
        {
            var existingUser = await _userManager.FindByNameAsync(dto.Username);
            if (existingUser != null)
            {
                return Conflict(new { message = "Username already exists." });
            }

            var user = new AppUser { UserName = dto.Username};
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok(new { message = "Admin Added" });
            }

            return BadRequest(result.Errors);
        }

        // PUT /api/admin/change-password
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

        // DELETE /api/admin/users/{id}
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteAdmin([FromRoute] string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { message = "Admin Deleted" });
        }


        /* ============== */

        // Admin View All Enrollment List API
        // GET /api/admin/enrollments
        [HttpGet("enrollments")]
        public async Task<IActionResult> GetAllEnroll()
        {
            var enrollments = await _enrollmentServices.GetAllEnrollmentAsync();

            var dtoList = enrollments.Select(e => new EnrollmentDto
            {
                EnrollmentId = e.EnrollmentId,
                CourseName = e.Course?.Name,
                Status = e.Status,
                EnrolledDate = e.EnrolledDate
            }).ToList();

            return Ok(dtoList);

        }

        // Admin Approve Student's Enrollment API
        // POST /api/admin/enrollments/{id}/approve
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveEnroll(string id, [FromBody] EnrollRequestDto dto)
        {
            var result = await _enrollmentServices.ApproveEnrollment(id);
            if (!result)
                return BadRequest(new { message = "Approval failed"});

            return Ok(new { message = "Enrollment Approved" });
        }

        // Admin Reject Student's Enrollment API
        // POST /api/admin/enrollments/{id}/reject
        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectEnroll(string id, [FromBody] EnrollRequestDto dto)
        {
            var result = await _enrollmentServices.RejectEnrollment(id);
            if (!result)
                return BadRequest(new { message = "Rejection failed" });

            return Ok(new { message = "Enrollment Rejected" });
        }
    }
}
