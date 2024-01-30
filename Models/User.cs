namespace LibraryApp.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Class { get; set; }
        public string? SIGE { get; set; }
        public bool HasBorrow { get; set; } = false;
        public bool HasReservation { get; set; } = false;
    }
}
