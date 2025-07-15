using NotificationService.Interfaces;
using NotificationService.Models.Email;
using NotificationService.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace NotificationService.Dtos
{
    public record ContentDto : BaseEmailMessage
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The email format is not valid.")]
        public override string To { get; init; }
        [Required(ErrorMessage = "Subject is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Subject must be between 2 and 100 characters.")]
        public string Subject { get; init; }

        [Required(ErrorMessage = "Body is required.")]
        [StringLength(2000, MinimumLength = 5, ErrorMessage = "Body must be between 5 and 2000 characters.")]
        public string Body { get; init; }

        public override EmailType EmailType { get; init; } = EmailType.Message;
    }
}
