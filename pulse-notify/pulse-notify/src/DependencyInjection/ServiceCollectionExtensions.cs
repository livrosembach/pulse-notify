using Microsoft.Extensions.DependencyInjection;
using NotifyKit.Abstractions;
using NotifyKit.Channels;
using NotifyKit.Handlers;
using NotifyKit.Observers;
using NotifyKit.Strategies;
using NotifyKit.Factories;
namespace NotifyKit.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotifyKit(this IServiceCollection services, Action<NotifyKitOptions>? configure = null)
    {
        var options = new NotifyKitOptions(); configure?.Invoke(options);
        services.AddSingleton<INotificationChannelFactory, NotificationChannelFactory>();
        services.AddSingleton<INotificationChannel>(sp => sp.GetRequiredService<INotificationChannelFactory>().Create("email"));
        services.AddSingleton<INotificationChannel>(sp => sp.GetRequiredService<INotificationChannelFactory>().Create("sms"));
        services.AddSingleton<INotificationChannel>(sp => sp.GetRequiredService<INotificationChannelFactory>().Create("push"));
        services.AddSingleton<INotificationChannel>(sp => sp.GetRequiredService<INotificationChannelFactory>().Create("console"));
        services.AddSingleton<InMemoryHistoryObserver>().AddSingleton<INotificationObserver>(sp => sp.GetRequiredService<InMemoryHistoryObserver>());
        services.AddSingleton<IRetryBackoffPolicy>(_ => new ExponentialBackoffPolicy(TimeSpan.FromMilliseconds(100)));
        services.AddSingleton<ChannelRegistry>();
        services.AddSingleton<NotificationHandlerChain>(_ => new NotificationHandlerChain(new[] { new ValidationHandler() }.Concat(options.Handlers)));
        services.AddSingleton<Notifier>();
        return services;
    }
}
