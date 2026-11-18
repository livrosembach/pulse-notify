# NotifyKit

NotifyKit é uma biblioteca extensível em .NET para envio de notificações por múltiplos canais. Inclui canais de email, SMS, push e console, integração com injeção de dependência, validação e templates, retry com backoff exponencial, histórico em memória, agendamento e processamento de comandos por prioridade.

## Alunos

- Alexandre Andrioli Tucci
- Livia Rosembach Oliveira
- Lucas Zanlorenzi Brisch

## Vídeo de apresentação

https://youtu.be/1XxT-GpjoQ8

## Como usar a biblioteca

Requisitos: .NET 8 SDK.

Adicione uma referência ao projeto da biblioteca:

```xml
<ProjectReference Include="..\pulse-notify\pulse-notify.csproj" />
```

Instale a abstração de injeção de dependência no projeto consumidor:

```bash
dotnet add package Microsoft.Extensions.DependencyInjection
```

Configure o NotifyKit e envie uma notificação:

```csharp
using Microsoft.Extensions.DependencyInjection;
using NotifyKit;
using NotifyKit.DependencyInjection;
using NotifyKit.Models;

var services = new ServiceCollection();
services.AddNotifyKit();

using var provider = services.BuildServiceProvider();
var notifier = provider.GetRequiredService<Notifier>();

var result = await notifier.SendAsync(
    Notification.Console()
        .Message("Olá do NotifyKit"));

Console.WriteLine(result.Success);
```

Canais disponíveis: `console`, `email`, `sms` e `push`. Para envio, o `Notifier` valida e prepara a notificação, coloca seu comando em uma fila e retorna quando a execução terminar. Entre comandos pendentes, a fila seleciona primeiro `Critical`, `High`, `Normal` e `Low`, mantendo a ordem de entrada entre notificações de mesma prioridade. O comando já em execução não é interrompido; um worker processa um comando por vez.

## Construtor de notificações

Use `Notification.Email`, `Notification.Sms`, `Notification.Push` ou `Notification.Console` para os canais prontos. Para um canal próprio, use `Notification.For(nomeDoCanal, destinatario)`. O builder oferece:

| Método | Finalidade |
|---|---|
| `Subject(string)` | Define um assunto opcional |
| `Message(string)` | Define o conteúdo da mensagem |
| `Priority(NotificationPriority)` | Define a prioridade na fila |
| `ScheduleAt(DateTimeOffset)` | Agenda a execução para uma data futura |
| `Template(string)` | Seleciona um template nomeado |

## Como estender a biblioteca

### Adicionar um canal

Implemente `INotificationChannel` e registre a implementação no mesmo contêiner de DI:

```csharp
public sealed class TelegramChannel : INotificationChannel
{
    public string Name => "telegram";

    public Task<NotificationResult> SendAsync(
        Notification notification,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Telegram para {notification.Recipient}: {notification.Message}");
        return Task.FromResult(NotificationResult.Succeeded(notification));
    }
}

services.AddNotifyKit();
services.AddSingleton<INotificationChannel, TelegramChannel>();
```

Envie usando `Notification.For("telegram", "@cliente").Message("Pedido aprovado")`. O registro aplica a política padrão de retry também a canais envoltos por outros decorators.

### Adicionar um decorator

Derive de `NotificationChannelDecorator` para adicionar comportamento a um canal sem modificá-lo. `LoggingDecorator` registra o envio, e `RetryDecorator` usa uma `IRetryBackoffPolicy`. Se um canal decorado não possuir `RetryDecorator`, o `ChannelRegistry` aplica o retry padrão ao pipeline. Ao registrar um canal decorado, substitua o registro direto do mesmo canal para evitar nomes duplicados:

```csharp
using NotifyKit.Decorators;

services.AddSingleton<INotificationChannel>(
    new LoggingDecorator(new TelegramChannel(), Console.WriteLine));
```

### Adicionar handlers e observers

Implemente `INotificationHandler` para validar, transformar ou enriquecer a notificação antes de ela entrar na fila. Registre handlers na configuração; o `ValidationHandler` padrão é executado primeiro:

```csharp
services.AddNotifyKit(options =>
    options.AddHandler(new AddDefaultSubjectHandler()));
```

Implemente `INotificationObserver` para receber os resultados ao final do envio e registre-o como `INotificationObserver` no contêiner. `InMemoryHistoryObserver` é o observer pronto que mantém o histórico em memória.

## Padrões de projeto

| Padrão | Implementação | Aplicação |
|---|---|---|
| Builder | `NotificationBuilder` | Monta notificações por uma API fluente |
| Factory Method | `INotificationChannelFactory` e `NotificationChannelFactory` | Cria os canais padrão |
| Decorator | `NotificationChannelDecorator`, `RetryDecorator` e `LoggingDecorator` | Acrescenta retry e logging sem alterar os canais |
| Chain of Responsibility | `NotificationHandlerChain` e `INotificationHandler` | Executa validações e handlers de pré-envio em sequência |
| Command | `NotificationCommand` | Empacota a notificação para execução pela fila |
| Strategy | `IRetryBackoffPolicy` e `ExponentialBackoffPolicy` | Permite variar a política de espera entre tentativas |
| Observer | `INotificationObserver` e `InMemoryHistoryObserver` | Notifica interessados quando o envio termina |
| Registry | `ChannelRegistry` | Localiza canais pelo nome |

## Executar a demo

Na raiz do repositório:

```bash
dotnet build pulse-notify.sln
dotnet run --project NotifyKit.Demo/NotifyKit.Demo.csproj
```
