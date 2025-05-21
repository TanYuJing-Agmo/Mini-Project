namespace Mini_Project.Dtos
{
    public class AddAdminDto
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class GetAdminDto
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
    }
}
