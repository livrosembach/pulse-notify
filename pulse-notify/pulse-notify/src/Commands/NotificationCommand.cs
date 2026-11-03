using NotifyKit.Abstractions;
using NotifyKit.Channels;
using NotifyKit.Models;
namespace NotifyKit.Commands;
public sealed class NotificationCommand(Notification n, ChannelRegistry channels, IEnumerable<INotificationObserver> observers)
{
    public NotificationPriority Priority => n.Priority;
    public async Task<NotificationResult> ExecuteAsync(CancellationToken ct = default)
    {
        if (n.ScheduledAt is { } time && time > DateTimeOffset.UtcNow) await Task.Delay(time - DateTimeOffset.UtcNow, ct);
        var result = await channels.Get(n.Channel).SendAsync(n, ct);
        foreach (var observer in observers) await observer.OnCompletedAsync(result, ct);
        return result;
    }
}
