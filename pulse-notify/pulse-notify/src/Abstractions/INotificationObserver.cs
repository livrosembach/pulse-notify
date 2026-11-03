using NotifyKit.Models;
namespace NotifyKit.Abstractions;
public interface INotificationObserver { Task OnCompletedAsync(NotificationResult result, CancellationToken cancellationToken = default); }
