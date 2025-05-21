using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_Project.Data;
using Mini_Project.Dtos;
using Mini_Project.Models;
using Mini_Project.Services;

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
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AdminController
            (
            IEnrollmentServices enrollmentServices,
            UserManager<AppUser> userManager,
            IConfiguration configuration,
            AppDbContext context,
            IMapper mapper
            )
        {
            _enrollmentServices = enrollmentServices;
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
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
            var dto = _mapper.Map<List<GetAdminDto>>(admins);

            return Ok(dto);
        }

        // POST /api/admin/add-admin
        [HttpPost("add-admin")]
        public async Task<IActionResult> AddAdmin([FromBody] AddAdminDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Username already exists." });
            }

            var user = new AppUser { UserName = dto.Username, Email = dto.Email};
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok(new { message = "Admin Added" });
            }

            return BadRequest(result.Errors);
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
            var dtoMapped = _mapper.Map<List<EnrollmentDto>>(enrollments);
            return Ok(dtoMapped);
        }

        // Admin Approve Student's Enrollment API
        // POST /api/admin/enrollments/approve
        [HttpPost("approve")]
        public async Task<IActionResult> ApproveEnroll([FromBody] StatusDto dto)
        {
            var result = await _enrollmentServices.ApproveEnrollment(dto.EnrollmentId);
            if (!result)
                return BadRequest(new { message = "Approval failed"});

            return Ok(new { message = "Enrollment Approved" });
        }

        // Admin Reject Student's Enrollment API
        // POST /api/admin/enrollments/reject
        [HttpPost("reject")]
        public async Task<IActionResult> RejectEnroll([FromBody] StatusDto dto)
        {
            var result = await _enrollmentServices.RejectEnrollment(dto.EnrollmentId);
            if (!result)
                return BadRequest(new { message = "Rejection failed" });

            return Ok(new { message = "Enrollment Rejected" });
        }
    }
}
