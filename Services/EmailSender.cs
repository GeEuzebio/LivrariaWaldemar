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

        public EmailSender(IOptions<AuthMessageSenderOptions> emailSettings)
        {
            _emailSettings = emailSettings?.Value ?? throw new ArgumentNullException(nameof(emailSettings));
        }

        public Task SendEmail(string email, string subject, string message)
        {

            var smtpClient = new SmtpClient
            {
                Host = "sandbox.smtp.mailtrap.io",
                Port = 2525,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("f7028f28c21a07", "a4b07042d70204"),
                EnableSsl = true
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
