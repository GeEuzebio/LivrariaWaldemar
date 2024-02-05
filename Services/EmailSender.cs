using LibraryApp.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LibraryApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly AuthMessageSenderOptions _emailSettings;
        private readonly IConfiguration _config;

        public EmailSender(IOptions<AuthMessageSenderOptions> emailSettings, IConfiguration config)
        {
            _emailSettings = emailSettings?.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            _config = config;
        }

        public Task SendEmail(string email, string subject, string message)
        {
            string? host = _config.GetSection("EmailSettings").GetSection("SmtpServer").Value;
            int port = int.Parse(_config.GetSection("EmailSettings").GetSection("SmtpPort").Value!);
            string? user = _config.GetSection("EmailSettings").GetSection("SmtpUser").Value;
            string? password = _config.GetSection("EmailSettings").GetSection("SmtpPassword").Value;
            bool ssl = bool.Parse(_config.GetSection("EmailSettings").GetSection("EnableSsl").Value!);
            var smtpClient = new SmtpClient
            {
                Host = host!,
                Port = port,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(user, password),
                EnableSsl = ssl
            };


            var mailMessage = new MailMessage
            {
                From = new MailAddress("georgeeuzebio@gmail.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            return smtpClient.SendMailAsync(mailMessage);
        }
        

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return SendEmail(email, subject, htmlMessage);
        }
    }
}
