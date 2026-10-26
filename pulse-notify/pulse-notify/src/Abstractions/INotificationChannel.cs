using pulse_notify.Models;

namespace pulse_notify.Abstractions;

public interface INotificationChannel
{
    string Name { get; }

    Task<NotificationResult> SendAsync(
        Notification notification,
        CancellationToken cancellationToken = default
    );
}