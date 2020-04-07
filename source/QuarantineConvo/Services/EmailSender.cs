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
            return Execute(subject, message, email);
        }

        public Task Execute(string subject, string message, string email)
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

            //just to make the function happy, gets the return value it wants
            //wont do anything because the api key is wrong
            string fakeKey = "aaa";
            var dummyClient = new SendGridClient(fakeKey);
            var msg = new SendGridMessage();
            return dummyClient.SendEmailAsync(msg);

        }


    }
}