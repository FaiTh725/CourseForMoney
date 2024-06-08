using MailKit.Net.Smtp;
using MimeKit;
using StudentPlacement.Backend.Models.Email;
using StudentPlacement.Backend.Services.Interfaces;

namespace StudentPlacement.Backend.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendDefaultEmail(string email, string subject, string message)
        {
            var emailAuth = configuration.GetSection("EmailAuthentification").Get<EmailAuthentification>();

            MimeMessage msg = new();

            msg.From.Add(new MailboxAddress("Распределение", emailAuth.Login));
            msg.To.Add(new MailboxAddress("Распределение", email));
            msg.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            
            msg.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 465);
                await client.AuthenticateAsync(emailAuth.Login, emailAuth.Password);
                await client.SendAsync(msg);
                await client.DisconnectAsync(true);
            }
        }
    }
}
