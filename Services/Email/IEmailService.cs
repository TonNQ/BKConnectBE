namespace BKConnectBE.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);

        string EncryptEmail(string emailAddress);

        string DecryptEmail(string encryptedEmail);

        string ConvertHtmlToString(string html);
        
        string MessageEmailForActiveAccount(string convertedHtml, string email, string link);
    }
}