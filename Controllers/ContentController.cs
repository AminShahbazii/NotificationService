using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Dtos;
using NotificationService.Interfaces;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly ILogger<ContentController> _logger;
        private readonly IEmailSenderService _emailSender;

        public ContentController(ILogger<ContentController> logger, IEmailSenderService emailSender)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }


        [HttpPost("custom")]
        public async Task<IActionResult> SendCustomEmailNotification([FromBody] ContentDto dto)
        {
            _logger.LogInformation("Received request to send custom email notification with content: {@ContentDto}", dto);


            if (dto == null)
            {
                _logger.LogError("ContentDto is null");
                return BadRequest("ContentDto cannot be null");
            }

            var content = new Models.Email.Content
            {
                Subject = dto.Subject,
                Body = dto.Body
            };

            try
            {
                await _emailSender.SendAsync(dto, content);
                _logger.LogInformation("Email sent successfully to {To}", dto.To);
                return Ok(new { Result = "Notification Sent", Content = content });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending custom email to {To}", dto.To);
                return StatusCode(500, "An error occurred while sending email");
            }
           





        }
    }
}
