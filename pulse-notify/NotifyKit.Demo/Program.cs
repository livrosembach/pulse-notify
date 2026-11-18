using Microsoft.Extensions.DependencyInjection;
using NotifyKit;
using NotifyKit.Abstractions;
using NotifyKit.DependencyInjection;
using NotifyKit.Models;

var services = new ServiceCollection();

services.AddNotifyKit();
services.AddSingleton<INotificationChannel, TelegramChannel>();

using var provider = services.BuildServiceProvider();
var notifier = provider.GetRequiredService<Notifier>();

await notifier.SendAsync(
    Notification.Console()
        .Message("NotifyKit funcionando"));

var result = await notifier.SendAsync(
    Notification.For("telegram", "@cliente")
        .Message("Seu pedido foi aprovado"));

Console.WriteLine($"Resultado do Telegram: {result.Success}");

public sealed class TelegramChannel : INotificationChannel
{
    public string Name => "telegram";

    public Task<NotificationResult> SendAsync(
        Notification notification,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine(
            $"[{Name}] -> {notification.Recipient}: {notification.Message}");

        return Task.FromResult(NotificationResult.Succeeded(notification));
    }
}