using System;
using System.Net;
using System.Net.Mail;

namespace GLPCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Today.ToString("D"));

            //enter your email
            string senderEmail = "**********";

            // find ur app pass
            string appPassword = "**********";

            //option to user env var 
            Environment.GetEnvironmentVariable("**********");

            var recipients = new List<string>
            {
                "**********",
                "**********",
                "**********"
            };

            string subject = "Quick update";
            string body = "Hi there,\n\nThis is my final test sent from my C# CLI.\n\n– Me";

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, appPassword)
            };

            int sent = 0, failed = 0;

            foreach (var to in recipients)
            {
                try
                {
                    using var msg = new MailMessage
                    {
                        From = new MailAddress(senderEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = false
                    };
                    msg.To.Add(to);

                    client.Send(msg);

                    Console.WriteLine($"Sent to {to}");
                    sent++;
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine($" {to} | SMTP: {ex.StatusCode} - {ex.Message}");
                    failed++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{to} | {ex.Message}");
                    failed++;
                }

                using var myMsg = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = "Email Sent Notifications",
                    Body = $"Failed to send: {failed}\nSuccessful sent: {sent}\nEmail list:\n{string.Join("\n", recipients)}",
                    IsBodyHtml = false
                };
                myMsg.To.Add(senderEmail);

                client.Send(myMsg);
            }
        }
    }
}