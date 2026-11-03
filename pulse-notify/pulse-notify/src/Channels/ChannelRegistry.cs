using NotifyKit.Abstractions;
using NotifyKit.Decorators;
namespace NotifyKit.Channels;
public sealed class ChannelRegistry(IEnumerable<INotificationChannel> channels, IRetryBackoffPolicy backoff)
{
    private readonly Dictionary<string, INotificationChannel> _channels = channels
        .Select(c => c is NotificationChannelDecorator ? c : new RetryDecorator(c, 3, backoff))
        .ToDictionary(c => c.Name, StringComparer.OrdinalIgnoreCase);
    public INotificationChannel Get(string name) => _channels.TryGetValue(name, out var c) ? c : throw new InvalidOperationException($"No channel registered with name '{name}'.");
}
