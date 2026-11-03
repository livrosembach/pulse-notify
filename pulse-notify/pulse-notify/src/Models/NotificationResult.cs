namespace NotifyKit.Models;

public sealed record NotificationResult(
    bool Success, string Channel, string Recipient, DateTimeOffset CompletedAt,
    string? ErrorMessage = null, int Attempts = 1)
{
    public static NotificationResult Succeeded(Notification n, int attempts = 1) => new(true, n.Channel, n.Recipient, DateTimeOffset.UtcNow, null, attempts);
    public static NotificationResult Failed(Notification n, string error, int attempts = 1) => new(false, n.Channel, n.Recipient, DateTimeOffset.UtcNow, error, attempts);
}