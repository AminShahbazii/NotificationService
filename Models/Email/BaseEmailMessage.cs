using NotificationService.Models.Enums;

namespace NotificationService.Models.Email
{
    public abstract record BaseEmailMessage
    {
        public abstract string To { get; init; }
        public abstract EmailType EmailType { get; init; }
    }
}
