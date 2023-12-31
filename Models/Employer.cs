namespace LibraryApp.Models
{
    public class Employer
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsAdmin { get; set; } = false;
        public string? Token { get; set; }

    }
}
