namespace LibraryApp.Models
{
    public class Book
    {
        public long BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public int? Year { get; set; }
        public string? Edition {  get; set; }
        public string Status { get; set; } = "Available";
        public long? UserId { get; set; } = null;

    }
}
