using NotifyKit.Channels;
using NotifyKit.Commands;
using NotifyKit.Handlers;
using NotifyKit.Models;
namespace NotifyKit;
public sealed class Notifier(ChannelRegistry channels, NotificationHandlerChain chain, IEnumerable<Abstractions.INotificationObserver> observers)
{
    public async Task<NotificationResult> SendAsync(Notification.Builder notification, CancellationToken ct = default)
    { var prepared = await chain.HandleAsync(notification.Build(), ct); return await new NotificationCommand(prepared, channels, observers).ExecuteAsync(ct); }
}
