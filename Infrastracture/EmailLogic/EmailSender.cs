using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastracture.EmailLogic
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<EmailOptions> options;

        public EmailSender(IOptions<EmailOptions> options)
        {
            this.options = options;
        }
        public void SendMail(string mail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(options.Value.FromEmail));
            email.To.Add(MailboxAddress.Parse(mail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect(options.Value.SmtpServer, options.Value.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(options.Value.FromEmail, options.Value.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
