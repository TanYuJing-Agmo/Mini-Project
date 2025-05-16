namespace Mini_Project.Dtos
{
    public class EnrollmentDto
    {
        public string EnrollmentId { get; set; }
        public string CourseName { get; set; }
        public string Status { get; set; }
        public DateTime EnrolledDate { get; set; }
    }

    public class enrollDto
    {
        public string? StudentId { get; set; }
        public int CourseId { get; set; }
    }

    public class withdrawDto
    {
        public string? EnrollmentId { get; set; }
    }
}
