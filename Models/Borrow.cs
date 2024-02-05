namespace LibraryApp.Models
{
    public class Borrow
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long BookId { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? LastDate { get; set; }
        public string? PublicKey { get; set; }
        public string? PrivateKey { get; set; }
        public bool IsDevolved { get; set; } = false;
    }
}
