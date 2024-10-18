using System;
using System.Threading.Tasks;

namespace EmailService.Services
{
    public interface IEmailService
    {
        Task SendEmailAddressConfirmationLink(Guid userId, string userEmail, string userName, string token);
    }
}
