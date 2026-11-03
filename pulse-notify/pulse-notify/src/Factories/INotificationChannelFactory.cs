using NotifyKit.Abstractions;

namespace NotifyKit.Factories;

/// <summary>
/// Creates the channels supplied by NotifyKit out of the box.
/// </summary>
public interface INotificationChannelFactory
{
    INotificationChannel Create(string channelName);
}
