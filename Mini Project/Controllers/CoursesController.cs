using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mini_Project.Dtos;
using Mini_Project.Models;
using Mini_Project.Services;

namespace Mini_Project.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CoursesController : Controller
    {
        private readonly IEnrollmentServices _enrollmentServices;
        private readonly ICourseServices _courseServices;

        public CoursesController(IEnrollmentServices enrollmentServices, ICourseServices courseServices)
        {
            _enrollmentServices = enrollmentServices;
            _courseServices = courseServices;
        }

        // Admin Endpoints
        //CRUD Courses
        //GET /api/get-all-course
        //[Authorize(Roles = "Admin")]
        [HttpGet("get-all-course")]
        public async Task<IActionResult> GetAllCourse()
        {
            var course = await _courseServices.GetAllCoursesAsync();
            if (course == null)
                return NotFound();

            return Ok(course);
        }

        // POST /api/course
        //[Authorize(Roles = "Admin")]
        [HttpPost("add-course")]
        public async Task<IActionResult> AddCourse([FromBody] courseDto dto)
        {
            var existing = await _courseServices.GetCourseByNameAsync(dto.Name);
            if (existing != null)
                return BadRequest(new { message = "The course already exists" });

            var course = new Course
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var result = await _courseServices.AddCourseAsync(course);
            return Ok(new { message = "Course Added Successfully" });
        }

        // PUT /api/course/{id}
        //[Authorize(Roles = "Admin")]
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> UpdateCourse([FromBody] courseDto dto)
        {
            var course = await _courseServices.GetCourseByIdAsync(dto.Id);
            if (course == null)
                return NotFound();

            course.Name = dto.Name;
            course.Description = dto.Description;

            var result = await _courseServices.UpdateCourseAsync(course);
            if (!result)
                return BadRequest(new {message = "Course Update Failed"});

            return Ok(new { message = "Course Updated Successfully" });
        }

        // DELETE /api/course/{id}
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {
            var course = await _courseServices.GetCourseByIdAsync(id);
            if (course == null)
                return NotFound();

            var result = await _courseServices.DeleteCourseAsync(id);
            if (!result)
                return BadRequest(new { message = "Course Deletion Failed" });

            return Ok(new { message = "Course Deleted Successfully" });
        }

        // Student Endpoints
        // Student Enroll Course API
        // POST /api/course/enroll
        //[Authorize(Roles = "Student")]
        [Route("enroll")]
        [HttpPost]
        public async Task<IActionResult> StudentEnroll([FromBody] enrollDto dto)
        {
            var existing = await _courseServices.GetCourseByIdAsync(dto.CourseId);
            if (existing == null)
                return BadRequest(new { message = "The course does not exist" });

            var result = await _enrollmentServices.EnrollCourseAsync(dto.StudentId, dto.CourseId);
            if (!result)
                return BadRequest(new { message = "Enrollment request failed" });

            return Ok(new { message = "Enrollment request submitted successfully" });
        }

        // Student Withdraw Course API
        // POST /api/course/withdraw
        //[Authorize(Roles = "Student")]
        [Route("withdraw")]
        [HttpPost]
        public async Task<IActionResult> WithdrawEnroll([FromBody] withdrawDto dto)
        {
            var result = await _enrollmentServices.WithdrawEnrollment(dto.EnrollmentId);
            if (!result)
                return BadRequest(new { message = "Do not have this enrollment" });

            return Ok(new { message = "Enrollment Withdrawn Successfully" });
        }
    }
}
