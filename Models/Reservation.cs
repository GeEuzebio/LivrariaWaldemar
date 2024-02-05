namespace LibraryApp.Models
{
    public class Reservation
    {
        public long Id { get; set; }
        public string? UserId { get; set; }
        public string? BookId { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? LastDate { get; set; }
    }
}
