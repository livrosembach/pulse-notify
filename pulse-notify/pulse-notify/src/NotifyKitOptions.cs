using NotifyKit.Abstractions;
namespace NotifyKit;
public sealed class NotifyKitOptions
{
    internal List<INotificationHandler> Handlers { get; } = [];
    public NotifyKitOptions AddHandler(INotificationHandler handler) { Handlers.Add(handler); return this; }
}
