using NotifyKit.Builders;

namespace NotifyKit.Models;

public sealed record Notification
{
    internal Notification(string recipient, string channel, string? subject, string message,
        NotificationPriority priority, DateTimeOffset? scheduledAt, string? templateName)
    { Recipient = recipient; Channel = channel; Subject = subject; Message = message; Priority = priority; ScheduledAt = scheduledAt; TemplateName = templateName; }
    public string Recipient { get; }
    public string Channel { get; }
    public string? Subject { get; }
    public string Message { get; init; }
    public NotificationPriority Priority { get; }
    public DateTimeOffset? ScheduledAt { get; }
    public string? TemplateName { get; }
    public static NotificationBuilder Email(string recipient) => new(recipient, "email");
    public static NotificationBuilder Sms(string recipient) => new(recipient, "sms");
    public static NotificationBuilder Push(string recipient) => new(recipient, "push");
    public static NotificationBuilder Console(string recipient = "console") => new(recipient, "console");
    public static NotificationBuilder For(string channel, string recipient) => new(recipient, channel);
}