namespace LibraryApp.Models
{
    public class Borrow
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? BookTitle { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? LastDate { get; set; }
    }
}
