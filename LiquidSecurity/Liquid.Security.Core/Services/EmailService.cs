using Microsoft.AspNet.Identity;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Liquid.Security.Services
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var configuration = SecurityProvider.Email;

            var smtpClient = new SmtpClient(configuration.Host, 25);
            smtpClient.Credentials = new NetworkCredential(configuration.Username, configuration.Password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = true;

            var mail = new MailMessage();
            mail.To.Add(message.Destination);
            mail.From = new MailAddress(configuration.SendFrom, configuration.SendFromName);
            mail.Subject = message.Subject;
            mail.Body = message.Body;

            smtpClient.Send(mail);

            return Task.FromResult(0);
        }
    }
}
