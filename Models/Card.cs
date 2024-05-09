namespace LibraryApp.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public long UserId { get; set; }
    }
}
