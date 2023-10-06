using System.Net;
using System.Net.Mail;
using System.Text;

namespace BKConnectBE.Service.Email
{
    public class EmailService : IEmailService
    {
        private readonly Emailing _configSection;

        public EmailService(IConfiguration configuration)
        {
            _configSection = configuration.GetSection("Settings").Get<Settings>().Emailing;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            SmtpClient client = new()
            {
                Host = _configSection.Host,
                Port = _configSection.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = _configSection.UserName,
                    Password = _configSection.Password
                }
            };
            MailMessage mailMsg = new()
            {
                From = new MailAddress(_configSection.UserName),
                Subject = subject,
                IsBodyHtml = true,
                Body = message
            };
            mailMsg.To.Add(email);

            await client.SendMailAsync(mailMsg);
        }

        public string EncryptEmail(string emailAddress)
        {
            byte[] data = Encoding.UTF8.GetBytes(emailAddress);
            string encryptedEmail = Convert.ToBase64String(data);
            return encryptedEmail;
        }

        public string DecryptEmail(string encryptedEmail)
        {
            byte[] data = Convert.FromBase64String(encryptedEmail);
            string decryptedEmail = Encoding.UTF8.GetString(data);
            return decryptedEmail;
        }

        public string ConvertHtmlToString(string html)
        {
            StreamReader sr = new(html);
            string s = sr.ReadToEnd();
            s = s.Replace("\r\n", "\n");
            return s;
        }

        public string MessageEmailForActiveAccount(string convertedHtml, string email, string link)
        {
            var s = convertedHtml;
            s = s.Replace("{email}", email);
            s = s.Replace("{link}", link);
            return s;
        }
    }
}