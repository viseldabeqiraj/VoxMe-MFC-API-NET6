using MailKit.Net.Smtp;
using MFC_VoxMe.Core.Dtos.Email;
using MFC_VoxMe.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private EmailConfiguration? _emailConfiguration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void EmailDataFromConfig()
        {
            _emailConfiguration = _configuration.GetSection
                ("EmailConfiguration").Get<EmailConfiguration>(); 
        }
        public bool SendEmail(EmailMessage emailData)
        {
                EmailDataFromConfig();
                MimeMessage emailMessage = new MimeMessage();

                MailboxAddress emailFrom = new MailboxAddress(_emailConfiguration?.Name, _emailConfiguration?.EmailId);
                emailMessage.From.Add(emailFrom);

                MailboxAddress emailTo = new MailboxAddress(emailData.EmailToName, emailData.EmailToId);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = emailData.EmailSubject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = emailData.EmailBody;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                SmtpClient emailClient = new SmtpClient();
                emailClient.Connect(_emailConfiguration?.Host, _emailConfiguration.Port, _emailConfiguration.UseSSL);
                emailClient.Authenticate(_emailConfiguration?.EmailId, _emailConfiguration?.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();

                return true;
        }
    }
}
