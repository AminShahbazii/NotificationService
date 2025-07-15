using NotificationService.Interfaces;
using NotificationService.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace NotificationService.Models.Email
{
    public record WelcomeEmailMessage : BaseEmailMessage
    {
       
        public override string To { get; init; }
        public string UserName { get; init; }
        public DateTime RegisteredAt { get; init; }
        public override EmailType EmailType { get; init; }
    }
}
