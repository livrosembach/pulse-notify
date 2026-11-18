using NotifyKit.Abstractions;
using NotifyKit.Models;

namespace NotifyKit.Handlers;

public sealed class ValidationHandler : INotificationHandler
{
    public Task<Notification> HandleAsync(Notification n, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(n.Message))
        {
            throw new ArgumentException("Message is required.");
        }

        return Task.FromResult(n);
    }
}

public sealed class TemplateHandler(IReadOnlyDictionary<string, string> templates) : INotificationHandler
{
    public Task<Notification> HandleAsync(Notification n, CancellationToken ct = default)
    {
        if (n.TemplateName is not null && templates.TryGetValue(n.TemplateName, out var message))
        {
            return Task.FromResult(n with { Message = message });
        }

        return Task.FromResult(n);
    }
}

public sealed class NotificationHandlerChain(IEnumerable<INotificationHandler> handlers)
{
    public async Task<Notification> HandleAsync(Notification n, CancellationToken ct = default)
    {
        foreach (var handler in handlers)
        {
            n = await handler.HandleAsync(n, ct);
        }

        return n;
    }
}
