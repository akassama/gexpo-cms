using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NgoExpoApp.Models.Emailer
{
    public class EmailService
    {
        //Function to send email using  sendgrid
        public static bool SendEmail(string from_email, string to_email, string email_subject, string message_body, string display_name)
        {
            AppFunctions functions = new AppFunctions();
            SystemConfiguration systemConfiguration = new SystemConfiguration();

            //Check if using sendgrid to send email
            var UseSendGrid = functions.GetConfigurationsData("UseSendGrid", "false");
            if (Convert.ToBoolean(UseSendGrid))
            {
                try
                {
                    var apiKey = functions.GetConfigurationsData("SendGridKey", systemConfiguration.sendGridKey);//get SendGrid api key
                    var client = new SendGridClient(apiKey);
                    var from = new EmailAddress(from_email, display_name);
                    var subject = email_subject;
                    var to = new EmailAddress(to_email, to_email.Split("@")[0]);
                    var plainTextContent = functions.StripHTML(message_body);
                    var htmlContent = message_body;
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    var response = client.SendEmailAsync(msg);

                    return true;
                }
                catch (Exception ex)
                {
                    //TODO log error
                    Console.WriteLine(ex);
                    return false;
                }

            }
            else
            {
                //get smtp email settings
                string smtp_host = functions.GetConfigurationsData("SmtpHost", systemConfiguration.smtpHost);
                string smtp_email = functions.GetConfigurationsData("SmtpEmail", systemConfiguration.smtpEmail);
                string smtp_pass = functions.GetConfigurationsData("SmtpPassword", systemConfiguration.smtpPass);
                int smtp_port = functions.Int32Parse(functions.GetConfigurationsData("SmtpPort", systemConfiguration.smtpPort.ToString()), 547);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(smtp_host);
                mail.From = new MailAddress(from_email, display_name);
                mail.To.Add(to_email);
                mail.Subject = email_subject;
                mail.Body = message_body;
                mail.IsBodyHtml = true;


                SmtpServer.Port = smtp_port;
                SmtpServer.Credentials = new NetworkCredential(smtp_email, smtp_pass);
                SmtpServer.EnableSsl = true;

                try
                {
                    SmtpServer.Send(mail);
                    return true;
                }
                catch (Exception ex)
                {
                    //TODO Log Error
                    Console.WriteLine(ex);
                    return false;
                }
            }



        }
    }
}
