using NotifyKit.Abstractions;
namespace NotifyKit.Strategies;
public sealed class ExponentialBackoffPolicy(TimeSpan initialDelay, TimeSpan? maximumDelay = null) : IRetryBackoffPolicy
{
    private readonly TimeSpan _maximumDelay = maximumDelay ?? TimeSpan.FromSeconds(30);
    public TimeSpan GetDelay(int retryNumber) => TimeSpan.FromMilliseconds(Math.Min(_maximumDelay.TotalMilliseconds, initialDelay.TotalMilliseconds * Math.Pow(2, Math.Max(0, retryNumber - 1))));
}
