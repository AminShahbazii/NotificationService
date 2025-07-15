using MailKit.Security;
using MimeKit;
using NotificationService.Interfaces;
using NotificationService.Models.Email;
using NotificationService.Models.Enums;


namespace NotificationService.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSenderService> _logger;
        public EmailSenderService(IConfiguration config, ILogger<EmailSenderService> logger)
        {
            _config = config;
            _logger = logger;
        }
        public async Task SendAsync<T>(T msg, Content content) where T : BaseEmailMessage
        {

            if (string.IsNullOrWhiteSpace(msg.To))
            {
                _logger.LogWarning("Email recipient is empty. Aborting send.");
                return;
            }

            _logger.LogInformation("Starting to send email to {To}", msg.To);


            /// explain /// Create a new MimeMessage to represent the email

            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(_config["Email:From"]));

            message.To.Add(MailboxAddress.Parse(msg.To));

            /// explain /// Set the subject of the email based on the template type

            message.Subject = content.Subject;

            message.Body = new TextPart("plain")
            {
                        Text = content.Body
            };



            try
            {

                /// explain /// Connect to the SMTP server and authenticate using the provided credentials

                using var client = new MailKit.Net.Smtp.SmtpClient();

                await client.ConnectAsync(
                    _config["Email:Host"] ?? throw new InvalidOperationException("SMTP host is not configured."),
                    int.Parse(_config["Email:Port"] ?? throw new InvalidOperationException("SMTP port is not configured")),
                    SecureSocketOptions.StartTls);
                          

                await client.AuthenticateAsync(
                    _config["Email:Username"] ?? throw new InvalidOperationException("SMTP username is not configured"),
                    _config["Email:Password"] ?? throw new InvalidOperationException("SMTP password is not configured"));

                await client.SendAsync(message);

                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully to {To} with Subject: {Subject}", msg.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", msg.To);
                throw;
            }
        }
    }
}
