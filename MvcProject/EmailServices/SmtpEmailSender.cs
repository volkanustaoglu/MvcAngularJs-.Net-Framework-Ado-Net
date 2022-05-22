using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace MvcProject.EmailServices
{
    public class SmtpEmailSender
    {

        public static void MailSender(string Email,string MsgSubject, string Message)
        {
            var fromAddress = "admin@test.com";
            string subject = "test | "+ MsgSubject;
            using (var smtp = new SmtpClient
            {
                Host = "smtp.test.com",
                Port = 587,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress, "3333")
            })
            {
                using (var message = new MailMessage(fromAddress, Email, subject, Message)
                {
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }
              
            }
        }

    }
}