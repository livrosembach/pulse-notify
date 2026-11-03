using NotifyKit.Models;

namespace NotifyKit.Builders;

/// <summary>
/// Fluent Builder that assembles an immutable notification.
/// </summary>
public sealed class NotificationBuilder
{
    private readonly string _recipient;
    private readonly string _channel;
    private string? _subject;
    private string _message = string.Empty;
    private NotificationPriority _priority = NotificationPriority.Normal;
    private DateTimeOffset? _scheduledAt;
    private string? _templateName;

    internal NotificationBuilder(string recipient, string channel)
    {
        _recipient = string.IsNullOrWhiteSpace(recipient)
            ? throw new ArgumentException("Recipient is required.", nameof(recipient))
            : recipient;
        _channel = channel;
    }

    public NotificationBuilder Subject(string subject) { _subject = subject; return this; }
    public NotificationBuilder Message(string message) { _message = message; return this; }
    public NotificationBuilder Priority(NotificationPriority priority) { _priority = priority; return this; }
    public NotificationBuilder ScheduleAt(DateTimeOffset scheduledAt) { _scheduledAt = scheduledAt; return this; }
    public NotificationBuilder Template(string templateName) { _templateName = templateName; return this; }

    public Notification Build() =>
        new(_recipient, _channel, _subject, _message, _priority, _scheduledAt, _templateName);

    public static implicit operator Notification(NotificationBuilder builder) => builder.Build();
}
