namespace Mini_Project.Dtos
{
    public class EnrollmentDto
    {
        public string EnrollmentId { get; set; }
        public string UserName { get; set; }
        public string CourseName { get; set; }
        public string Status { get; set; }
        public DateTime EnrolledDate { get; set; }
    }

    public class EnrollDto
    {
        public int CourseId { get; set; }
    }

    public class StatusDto
    {
        public string? EnrollmentId { get; set; }
    }
}
