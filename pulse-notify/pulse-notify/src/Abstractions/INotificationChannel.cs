using NotifyKit.Models;

namespace NotifyKit.Abstractions;

public interface INotificationChannel
{
    string Name { get; }

    Task<NotificationResult> SendAsync(
        Notification notification,
        CancellationToken cancellationToken = default
    );
}