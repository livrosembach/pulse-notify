namespace pulse_notify.Models;

public sealed record Notification
{
    private string Recipient;
    private string Subject;
    private string Message;
    private string Channel;
    private NotificationPriority Priority = NotificationPriority.Normal;
};