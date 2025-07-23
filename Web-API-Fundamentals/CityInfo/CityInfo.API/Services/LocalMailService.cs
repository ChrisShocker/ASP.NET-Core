namespace CityInfo.API.Services
{
    public class LocalMailService
    {
        private string _mailToAddress = "admin@mycompany.com";
        private string _mailFromAddress = "noreply@mycompany.com";

        public void Send(string subject, string message)
        {
            // here we would send the email using an SMTP client or similar
            // for now, we'll just simulate sending an email by writing to the console
            Console.WriteLine($"Mail from: {_mailFromAddress}");
            Console.WriteLine($"Mail to: {_mailToAddress}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
