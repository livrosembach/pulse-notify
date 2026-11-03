using NotifyKit.Models;
namespace NotifyKit.Abstractions;
public interface INotificationHandler { Task<Notification> HandleAsync(Notification notification, CancellationToken cancellationToken = default); }
