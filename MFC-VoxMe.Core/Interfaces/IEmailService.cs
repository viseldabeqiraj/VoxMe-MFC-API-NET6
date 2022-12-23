using MFC_VoxMe.Core.Dtos.Email;

namespace MFC_VoxMe.Core.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(EmailMessage emailData);
    }
}
