using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailMetaData emailMetaData;

        public EmailService(EmailMetaData metaData)
        {
            emailMetaData = metaData;
        }

        public bool Send(string to, string subject, string html)
        {
            // create message
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(emailMetaData.FromEmail));

            email.To.Add(MailboxAddress.Parse(to));

            email.Subject = subject;

            email.Body = new TextPart(TextFormat.Html) { Text = html };

            try
            {
                // send email
                using var smtp = new SmtpClient();

                smtp.Connect(emailMetaData.SmtpServer, emailMetaData.Port, SecureSocketOptions.Auto);

                smtp.Authenticate(emailMetaData.UserName, emailMetaData.Password);

                smtp.Send(email);

                smtp.Disconnect(true);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
