using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using Project.API.Helpers;

namespace Project.BLL.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly MailSettings _mailSettings;

        public EmailServices(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }



        public async Task<string> SendEmailAsync(string email, string message, string? reason)
        {
            try
            {
                //sending the Message of passwordResetLink
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate("abdalrhmanfathy170@gmail.com", "glrz yohs gkxx oxhn");
                    var bodybuilder = new BodyBuilder
                    {
                        HtmlBody = $"{message}",
                        TextBody = "wellcome",
                    };
                    var Message = new MimeMessage
                    {
                        Body = bodybuilder.ToMessageBody()
                    };
                    Message.From.Add(new MailboxAddress("Project", "abdalrhmanfathy170@gmail.com"));
                    Message.To.Add(new MailboxAddress("testing", email));
                    Message.Subject = reason == null ? "Not Sumbitted" : reason;
                    await client.SendAsync(Message);
                    await client.DisconnectAsync(true);
                }
                //end of sending email
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Email),
                Subject = subject
            };

            email.To.Add(MailboxAddress.Parse(mailTo));

            var builder = new BodyBuilder();

            if (attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in attachments)
                {
                    if (file.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();

                        object value = builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }

    }
}
