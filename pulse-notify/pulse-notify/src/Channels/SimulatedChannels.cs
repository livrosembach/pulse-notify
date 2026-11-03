using NotifyKit.Abstractions;
using NotifyKit.Models;
namespace NotifyKit.Channels;
public abstract class SimulatedChannelBase(string name) : INotificationChannel
{
    public string Name { get; } = name;
    public virtual Task<NotificationResult> SendAsync(Notification n, CancellationToken ct = default)
    { Console.WriteLine($"[{Name}] -> {n.Recipient}: {n.Message}"); return Task.FromResult(NotificationResult.Succeeded(n)); }
}
public sealed class EmailChannel : SimulatedChannelBase { public EmailChannel() : base("email") { } }
public sealed class SmsChannel : SimulatedChannelBase { public SmsChannel() : base("sms") { } }
public sealed class PushChannel : SimulatedChannelBase { public PushChannel() : base("push") { } }
public sealed class ConsoleChannel : SimulatedChannelBase { public ConsoleChannel() : base("console") { } }
