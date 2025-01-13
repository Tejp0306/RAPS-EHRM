using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHRM.ServiceLayer.Utility
{
    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);

        void SendEmailAsync(string toEmail, string ccEmail, string subject, string body);
    }
}
