# NotifyKit

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

Canais disponíveis: `console`, `email`, `sms` e `push`.
