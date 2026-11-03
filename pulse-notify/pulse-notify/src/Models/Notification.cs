namespace NotifyKit.Models;

public sealed record Notification
{
    private Notification(string recipient, string channel, string? subject, string message,
        NotificationPriority priority, DateTimeOffset? scheduledAt, string? templateName)
    { Recipient = recipient; Channel = channel; Subject = subject; Message = message; Priority = priority; ScheduledAt = scheduledAt; TemplateName = templateName; }
    public string Recipient { get; }
    public string Channel { get; }
    public string? Subject { get; }
    public string Message { get; init; }
    public NotificationPriority Priority { get; }
    public DateTimeOffset? ScheduledAt { get; }
    public string? TemplateName { get; }
    public static Builder Email(string recipient) => new(recipient, "email");
    public static Builder Sms(string recipient) => new(recipient, "sms");
    public static Builder Push(string recipient) => new(recipient, "push");
    public static Builder Console(string recipient = "console") => new(recipient, "console");
    public static Builder For(string channel, string recipient) => new(recipient, channel);

    public sealed class Builder
    {
        private readonly string _recipient; private readonly string _channel;
        private string? _subject; private string _message = string.Empty;
        private NotificationPriority _priority = NotificationPriority.Normal;
        private DateTimeOffset? _scheduledAt; private string? _templateName;
        internal Builder(string recipient, string channel) { _recipient = string.IsNullOrWhiteSpace(recipient) ? throw new ArgumentException("Recipient is required.", nameof(recipient)) : recipient; _channel = channel; }
        public Builder Subject(string subject) { _subject = subject; return this; }
        public Builder Message(string message) { _message = message; return this; }
        public Builder Priority(NotificationPriority priority) { _priority = priority; return this; }
        public Builder ScheduleAt(DateTimeOffset scheduledAt) { _scheduledAt = scheduledAt; return this; }
        public Builder Template(string templateName) { _templateName = templateName; return this; }
        public Notification Build() => new(_recipient, _channel, _subject, _message, _priority, _scheduledAt, _templateName);
        public static implicit operator Notification(Builder builder) => builder.Build();
    }
}