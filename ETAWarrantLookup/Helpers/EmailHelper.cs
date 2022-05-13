using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ETAWarrantLookup.Helpers
{
    /// <summary>
    /// DEPRECATED
    /// </summary>
    public class EmailHelper
    {
        public bool SendEmail(string userEmail, string confirmationLink)
        {
            var credentials = new NetworkCredential(
                          ConfigurationManager.AppSettings["smtpUserAccount"],
                          ConfigurationManager.AppSettings["mailUserPassword"]
                          );

            var smtpClient = new SmtpClient
            {
                Credentials = credentials,
            };


            //client.Credentials = new System.Net.NetworkCredential("care@yogihosting.com", "yourpassword");
            //client.Host = "smtpout.secureserver.net";
            //client.Port = 80;


            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("care@yogihosting.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;

           

            try
            {
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
            }
            return false;
        }
    }
}

