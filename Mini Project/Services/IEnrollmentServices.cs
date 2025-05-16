using Mini_Project.Models;

namespace Mini_Project.Services
{
    public interface IEnrollmentServices
    {
        
        Task<List<Enrollment>> GetAllEnrollmentAsync();
        Task<List<Enrollment>> GetSelfEnrollmentAsync(string studentId);
        Task<bool> ApproveEnrollment(string enrollmentId);
        Task<bool> RejectEnrollment(string enrollmentId);
        Task<bool> WithdrawEnrollment(string enrollmentId);
        Task<bool> EnrollCourseAsync(string studentId, int courseId);
        
    }
}
