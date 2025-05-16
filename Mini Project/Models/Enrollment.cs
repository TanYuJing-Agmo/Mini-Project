namespace Mini_Project.Models
{
    public class Enrollment
    {
        public string? EnrollmentId { get; set; }
        public string? StudentId { get; set; }
        public int CourseId { get; set; }
        public string? Status { get; set; } = "Pending"; //Pending, Approved, Rejected, Withdrawn
        public DateTime EnrolledDate { get; set; } = DateTime.UtcNow;
        public AppUser? Student { get; set; }
        public Course? Course { get; set; }
    }
}
