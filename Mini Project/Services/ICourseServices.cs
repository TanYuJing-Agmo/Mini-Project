using Mini_Project.Models;

namespace Mini_Project.Services
{
    public interface ICourseServices
    {
        Task<List<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByNameAsync(string name);
        Task<Course?> GetCourseByIdAsync(int courseId);
        Task<Course> AddCourseAsync(Course course);
        Task<bool> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(int id);
    }
}
