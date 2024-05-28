using CleanArchitecture.Core.DTOs.Email;
using System.Threading.Tasks;
using System.Net.Mail;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
