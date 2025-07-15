using MassTransit;
using NotificationService.Interfaces;
using NotificationService.Models.Email;
using NotificationService.Services;

namespace NotificationService.Consum
{
    public class EmailNotificationConsumer : IConsumer<WelcomeEmailMessage>
    {
        private readonly ILogger<EmailNotificationConsumer> _logger;
        private readonly IEmailSenderService _emailSender;

        public EmailNotificationConsumer(ILogger<EmailNotificationConsumer> logger, IEmailSenderService emailSender)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task Consume(ConsumeContext<WelcomeEmailMessage> context)
        {
            _logger.LogInformation("Consuming message: {@Message}", context.Message);

            var message = context.Message;


            var reContent = new Content
            {
                Subject = "🎉 Welcome to Our Platform!",
                Body = $"Hi {message.UserName},\n\nThank you for registering on {message.RegisteredAt:d}. We're excited to have you!"
            };


            try
            {
                await _emailSender.SendAsync(message, reContent);
                _logger.LogInformation($"Email sent to: {message.To}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending welcome email to {Email}", message.To);
                throw;
            }            
        }
    }
}
