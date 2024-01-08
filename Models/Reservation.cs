namespace LibraryApp.Models
{
    public class Reservation
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long BookId { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? LastDate { get; set; }
    }
}
