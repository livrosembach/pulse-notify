using NotifyKit.Abstractions;
using NotifyKit.Decorators;

namespace NotifyKit.Channels;

public sealed class ChannelRegistry(IEnumerable<INotificationChannel> channels, IRetryBackoffPolicy backoff)
{
    private readonly Dictionary<string, INotificationChannel> _channels = BuildRegistry(channels, backoff);

    public INotificationChannel Get(string name) =>
        _channels.TryGetValue(name, out var channel)
            ? channel
            : throw new InvalidOperationException($"No channel registered with name '{name}'.");

    private static Dictionary<string, INotificationChannel> BuildRegistry(
        IEnumerable<INotificationChannel> channels,
        IRetryBackoffPolicy backoff) =>
        channels
            .Select(channel => EnsureRetry(channel, backoff))
            .ToDictionary(channel => channel.Name, StringComparer.OrdinalIgnoreCase);

    private static INotificationChannel EnsureRetry(
        INotificationChannel channel,
        IRetryBackoffPolicy backoff)
    {
        for (var current = channel; current is NotificationChannelDecorator decorator; current = decorator.InnerChannel)
        {
            if (current is RetryDecorator)
            {
                return channel;
            }
        }

        return new RetryDecorator(channel, 3, backoff);
    }
}
