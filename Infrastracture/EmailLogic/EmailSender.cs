using Core.Interfaces.Logging;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Infrastracture.EmailLogic
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<EmailOptions> _options;
        private readonly IAppLogger _logger;

        public EmailSender(IOptions<EmailOptions> _options,IAppLogger _logger)
        {
            this._options = _options;
            this._logger = _logger;
        }
        public void SendMail(string mail, string subject, string body)
        {
            try{
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_options.Value.FromEmail));
                email.To.Add(MailboxAddress.Parse(mail));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();
                smtp.Connect(_options.Value.SmtpServer, _options.Value.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_options.Value.FromEmail, _options.Value.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.Critical(ex.Message);
                throw;
            }
            
        }
    }
}
