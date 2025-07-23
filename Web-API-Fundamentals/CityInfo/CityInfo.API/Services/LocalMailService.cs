namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private string _mailToAddress = string.Empty;
        private string _mailFromAddress = string.Empty;

        public LocalMailService(IConfiguration configuration)
        {
            // read the email addresses from configuration
            _mailToAddress = configuration["MailSettings:MailToAddress"] ?? _mailToAddress;
            _mailFromAddress = configuration["MailSettings:MailFromAddress"] ?? _mailFromAddress;
        }

        public void Send(string subject, string message)
        {
            // here we would send the email using an SMTP client or similar
            // for now, we'll just simulate sending an email by writing to the console
            Console.WriteLine(nameof(LocalMailService));
            Console.WriteLine($"Mail from: {_mailFromAddress}");
            Console.WriteLine($"Mail to: {_mailToAddress}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
