using NotifyKit.Abstractions;
using NotifyKit.Models;
namespace NotifyKit.Handlers;
public sealed class ValidationHandler : INotificationHandler
{
    public Task<Notification> HandleAsync(Notification n, CancellationToken ct = default) { if (string.IsNullOrWhiteSpace(n.Message)) throw new ArgumentException("Message is required."); return Task.FromResult(n); }
}
public sealed class TemplateHandler(IReadOnlyDictionary<string, string> templates) : INotificationHandler
{
    public Task<Notification> HandleAsync(Notification n, CancellationToken ct = default) => Task.FromResult(n.TemplateName is not null && templates.TryGetValue(n.TemplateName, out var m) ? n with { Message = m } : n);
}
public sealed class NotificationHandlerChain(IEnumerable<INotificationHandler> handlers)
{
    public async Task<Notification> HandleAsync(Notification n, CancellationToken ct = default) { foreach (var h in handlers) n = await h.HandleAsync(n, ct); return n; }
}
