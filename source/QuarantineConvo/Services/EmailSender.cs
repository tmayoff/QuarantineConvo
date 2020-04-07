using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace QuarantineConvo.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        //public Task Execute(string apiKey, string subject, string message, string email)
        //{
        //    var client = new SendGridClient(apiKey);
        //    var msg = new SendGridMessage()
        //    {
        //        From = new EmailAddress("dummySimon99@gmail.com", Options.SendGridUser),
        //        Subject = subject,
        //        PlainTextContent = message,
        //        HtmlContent = message
        //    };
        //    msg.AddTo(new EmailAddress(email));

        //    // Disable click tracking.
        //    // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        //    msg.SetClickTracking(false, false);

        //    return client.SendEmailAsync(msg);
        //}

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var theMessage = new MimeMessage();
            theMessage.From.Add(new MailboxAddress("Quarantine Convo", "dummysimon99@gmail.com"));
            theMessage.To.Add(new MailboxAddress("Quarantine Convo", "dummysimon99@gmail.com"));
            theMessage.To.Add(new MailboxAddress(email, email));

            theMessage.Subject = subject;
            theMessage.Body = new TextPart("plain")
            {
                Text = message
            };

            var client = new SmtpClient();
            
            client.Connect("smtp.gmail.com", 587, false);
            client.Authenticate("dummysimon99@gmail.com", "badPass123!");
            client.Send(theMessage);
            client.Disconnect(true);

            //not good down there, but need it for function to work
            var dummyClient = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            return dummyClient.SendEmailAsync(msg);

        }


    }
}