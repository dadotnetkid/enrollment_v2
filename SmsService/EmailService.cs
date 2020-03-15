using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Services
{
    public class EmailService
    {
        public Task SendAsync(IdentityMessage message)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

            //initialization
            var mailMessage = new System.Net.Mail.MailMessage();
            //from
            mailMessage.From = new System.Net.Mail.MailAddress("thesis.maker@gmail.com", "PLT College, Inc.");
            //to
            mailMessage.To.Add(new System.Net.Mail.MailAddress(message.Destination));
            mailMessage.Body = message.Body;
            mailMessage.Subject = message.Subject;
            mailMessage.IsBodyHtml = true;

            //client.SendAsync(mailMessage, null);
            Task task = Task.Run(new Action(async () =>
            {
                await client.SendMailAsync(mailMessage);

            }));
            return task;
        }

    }
}
