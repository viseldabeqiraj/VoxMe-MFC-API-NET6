using MailKit.Net.Smtp;
using MFC_VoxMe.Core.Dtos.Email;
using MFC_VoxMe.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;

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
                ("EmailConfiguration:From").Get<EmailConfiguration>(); 
        }
        public bool SendEmail(EmailMessage emailData)
        {
                EmailDataFromConfig();
                MimeMessage emailMessage = new MimeMessage();

                MailboxAddress emailFrom = new MailboxAddress(_emailConfiguration?.Name, _emailConfiguration?.EmailId);
                emailMessage.From.Add(emailFrom);

                MailboxAddress emailTo = new MailboxAddress(emailData.Name, emailData.EmailId);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = emailData.Subject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.TextBody = emailData.Body;
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
