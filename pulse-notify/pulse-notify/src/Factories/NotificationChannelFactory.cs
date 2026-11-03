using NotifyKit.Abstractions;
using NotifyKit.Channels;

namespace NotifyKit.Factories;

/// <summary>
/// Factory Method for the standard simulated channels.
/// Custom channels should be registered directly with dependency injection.
/// </summary>
public sealed class NotificationChannelFactory : INotificationChannelFactory
{
    public INotificationChannel Create(string channelName) =>
        channelName.Trim().ToLowerInvariant() switch
        {
            "email" => new EmailChannel(),
            "sms" => new SmsChannel(),
            "push" => new PushChannel(),
            "console" => new ConsoleChannel(),
            _ => throw new ArgumentException($"Unsupported built-in channel '{channelName}'.", nameof(channelName))
        };
}
