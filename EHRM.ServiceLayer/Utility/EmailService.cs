using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Utility
{
    public class EmailService
    {
        public void SendEmail(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress("arjunsingh8545999@gmail.com", "Arjun");
            var toAddress = new MailAddress(toEmail);
            string fromPassword = "jhsp tslg ofdp sjmb"; // Use an app-specific password if 2FA is enabled
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 587;

            var smtp = new SmtpClient
            {
                Host = smtpHost,
                Port = smtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true
            })
            {
                // Create the HTML view
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);

                // Add the HTML view to the message
                message.AlternateViews.Add(htmlView);

                smtp.Send(message);
            }
        }


    }
}
