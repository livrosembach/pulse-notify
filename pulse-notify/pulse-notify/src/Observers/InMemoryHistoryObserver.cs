using NotifyKit.Abstractions;
using NotifyKit.Models;
namespace NotifyKit.Observers;
public sealed class InMemoryHistoryObserver : INotificationObserver
{
    private readonly List<NotificationResult> _history = [];
    public IReadOnlyList<NotificationResult> History => _history.AsReadOnly();
    public Task OnCompletedAsync(NotificationResult result, CancellationToken ct = default) { _history.Add(result); return Task.CompletedTask; }
}
