using Mini_Project.Data;
using Mini_Project.Models;

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
            return _context.Enrollments.ToList();
        }
        /*public async Task<List<Enrollment>> GetSelfEnrollmentAsync(string studentId)
        {
            return _context.Enrollments.FirstOrDefault(s => s.StudentId == StudentId).ToList();
        }
        public async Task<bool> ApproveEnrollment(int enrollmentId)
        {

        }
        public async Task<bool> RejectEnrollment(int enrollmentId)
        {

        }
        public async Task<bool> WithdrawEnrollment(int enrollmentId)
        {

        }
        public async Task<bool> EnrollCourse(string studentId, int courseId)
        {

        }*/
    }
}
