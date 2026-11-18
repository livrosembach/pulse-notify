using NotifyKit.Channels;
using NotifyKit.Commands;
using NotifyKit.Handlers;
using NotifyKit.Models;
using NotifyKit.Builders;

namespace NotifyKit;

public sealed class Notifier(ChannelRegistry channels, NotificationHandlerChain chain, IEnumerable<Abstractions.INotificationObserver> observers)
{
    private readonly NotificationCommandQueue _queue = new();

    public async Task<NotificationResult> SendAsync(NotificationBuilder notification, CancellationToken ct = default)
    {
        var prepared = await chain.HandleAsync(notification.Build(), ct);
        var command = new NotificationCommand(prepared, channels, observers);
        return await _queue.EnqueueAsync(command, ct);
    }
}
