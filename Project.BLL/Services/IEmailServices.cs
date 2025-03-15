using Microsoft.AspNetCore.Http;

namespace Project.BLL.Services
{
    public interface IEmailServices
    {
        Task<string> SendEmailAsync(string email, string message, string? reason);
        Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null);

    }
}
