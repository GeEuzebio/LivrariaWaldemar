namespace LibraryApp.Models
{
    public class Book
    {
        public long BookId { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? Author { get; set; }
        public string? Register { get; set; }
        public Status Status { get; set; } = Status.Available;
        public Status Reserved { get; set; } = Status.Available;
        public string? UserId { get; set; } = null;

    }
}
