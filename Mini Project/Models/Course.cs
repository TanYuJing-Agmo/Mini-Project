﻿namespace Mini_Project.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
