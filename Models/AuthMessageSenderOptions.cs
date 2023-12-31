namespace LibraryApp.Models
{
    public class AuthMessageSenderOptions
    {
        public string? SmtpServer { get; set; }
        public string? SmtpPort { get; set; }
        public string? SmtpUser { get; set; }
        public string? SmtpPassword { get; set; }
        public string? EnableSsl { get; set; }
    }
}
