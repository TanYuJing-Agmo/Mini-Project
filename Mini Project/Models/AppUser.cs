using Microsoft.AspNetCore.Identity;

namespace Mini_Project.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
