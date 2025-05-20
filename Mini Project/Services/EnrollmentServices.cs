using Mini_Project.Data;
using Mini_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Mini_Project.Services
{
    public class EnrollmentServices : IEnrollmentServices
    {
        private readonly AppDbContext _context;

        public EnrollmentServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Enrollment>> GetAllEnrollmentAsync()
        {
            return await _context.Enrollments.ToListAsync();
        }
        public async Task<List<Enrollment>> GetSelfEnrollmentAsync(string studentId)
        {
            return await _context.Enrollments.Where(s => s.StudentId == studentId).ToListAsync();
        }
        public async Task<bool> ApproveEnrollment(string enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);

            if (enrollment == null)
            {
                return false;
            }

            enrollment.Status = "Approved";
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();

            return true;

        }
        public async Task<bool> RejectEnrollment(string enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);

            if (enrollment == null)
            {
                return false;
            }

            enrollment.Status = "Rejected";
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> WithdrawEnrollment(string enrollmentId)
        {
            var enrollment = await _context.Enrollments.FindAsync(enrollmentId);

            if (enrollment == null)
            {
                return false;
            }

            enrollment.Status = "Withdrawn";
            _context.Enrollments.Update(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> EnrollCourseAsync(string studentId, int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            var student = await _context.Users.FindAsync(studentId);

            if (course == null || student == null)
                return false;

            bool alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (alreadyEnrolled)
                return false;

            var enrollment = new Enrollment
            {
                EnrollmentId = Guid.NewGuid().ToString(),
                StudentId = studentId,
                CourseId = courseId,
                Status = "Pending",
                EnrolledDate = DateTime.UtcNow
            };

            await _context.Enrollments.AddAsync(enrollment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
