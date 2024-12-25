namespace Infrastracture.EmailLogic
{
    public interface IEmailSender
    {
        void SendMail(string mail, string subject, string body);
    }
}