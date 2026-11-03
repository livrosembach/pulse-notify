using NotifyKit.Abstractions;
using NotifyKit.Models;
namespace NotifyKit.Decorators;
public abstract class NotificationChannelDecorator(INotificationChannel inner) : INotificationChannel
{
    protected INotificationChannel Inner { get; } = inner;
    public string Name => Inner.Name;
    public abstract Task<NotificationResult> SendAsync(Notification n, CancellationToken ct = default);
}
public sealed class RetryDecorator(INotificationChannel inner, int maxRetries, IRetryBackoffPolicy backoff) : NotificationChannelDecorator(inner)
{
    public override async Task<NotificationResult> SendAsync(Notification n, CancellationToken ct = default)
    {
        for (var attempt = 1; attempt <= maxRetries + 1; attempt++)
        {
            try { var result = await Inner.SendAsync(n, ct); if (result.Success || attempt > maxRetries) return result with { Attempts = attempt }; }
            catch (Exception ex)
            {
                if (attempt > maxRetries) return NotificationResult.Failed(n, ex.Message, attempt);
            }
            await Task.Delay(backoff.GetDelay(attempt), ct);
        }
        return NotificationResult.Failed(n, "Retry limit reached.", maxRetries + 1);
    }
}
public sealed class LoggingDecorator(INotificationChannel inner, Action<string>? log = null) : NotificationChannelDecorator(inner)
{
    public override async Task<NotificationResult> SendAsync(Notification n, CancellationToken ct = default)
    { (log ?? Console.WriteLine)($"Sending {n.Channel} notification to {n.Recipient}."); var result = await Inner.SendAsync(n, ct); (log ?? Console.WriteLine)($"Notification {(result.Success ? "succeeded" : "failed")}."); return result; }
}
