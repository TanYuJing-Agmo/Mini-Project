namespace Mini_Project.Dtos
{
    public class AddCourseDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateCourseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
