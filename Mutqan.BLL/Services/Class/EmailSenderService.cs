using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Mutqan.BLL.Services.Class
{
    public class EmailSenderService : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromAddress = new MailAddress("dylyb7883@gmail.com", "Mutqan");

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("dylyb7883@gmail.com", "ezsh magz zzyw raby")
            };

            var mailMessage = new MailMessage
            {
                From = fromAddress,
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }
    }
}
