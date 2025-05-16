using Microsoft.EntityFrameworkCore;
using Mini_Project.Data;
using Mini_Project.Models;

namespace Mini_Project.Services
{
    public class CourseServices : ICourseServices
    {
        private readonly AppDbContext _context;

        public CourseServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetCourseByNameAsync(string name)
        {
            return await _context.Courses.FirstOrDefaultAsync(s => s.Name == name);
        }

        public async Task<Course?> GetCourseByIdAsync(int courseId)
        {
            return await _context.Courses.FirstOrDefaultAsync(s => s.CourseId == courseId);
        }

        public async Task<Course> AddCourseAsync(Course course)
        {
            var result = await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<bool> UpdateCourseAsync(Course course)
        {
            var existing = await _context.Courses.FindAsync(course.CourseId);

            if (existing == null)
                return false;

            existing.Name = course.Name;
            existing.Description = course.Description;

            _context.Courses.Update(existing);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var existing = await _context.Courses.FindAsync(id);
            if (existing == null) return false;

            _context.Courses.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
