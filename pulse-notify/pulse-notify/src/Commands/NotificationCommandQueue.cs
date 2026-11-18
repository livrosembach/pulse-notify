using NotifyKit.Models;

namespace NotifyKit.Commands;

internal sealed class NotificationCommandQueue
{
    private readonly object _sync = new();
    private readonly PriorityQueue<QueuedCommand, (int Priority, long Sequence)> _commands = new();
    private long _nextSequence;
    private bool _isProcessing;

    public Task<NotificationResult> EnqueueAsync(
        NotificationCommand command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var queuedCommand = new QueuedCommand(
            command,
            cancellationToken,
            new TaskCompletionSource<NotificationResult>(TaskCreationOptions.RunContinuationsAsynchronously));
        var shouldStartProcessor = false;

        lock (_sync)
        {
            var priority = -(int)command.Priority;
            _commands.Enqueue(queuedCommand, (priority, _nextSequence++));
            if (!_isProcessing)
            {
                _isProcessing = true;
                shouldStartProcessor = true;
            }
        }

        if (shouldStartProcessor)
        {
            _ = ProcessQueueAsync();
        }

        return queuedCommand.Completion.Task;
    }

    private async Task ProcessQueueAsync()
    {
        while (true)
        {
            QueuedCommand queuedCommand;
            lock (_sync)
            {
                if (_commands.Count == 0)
                {
                    _isProcessing = false;
                    return;
                }

                queuedCommand = _commands.Dequeue();
            }

            try
            {
                queuedCommand.CancellationToken.ThrowIfCancellationRequested();
                var result = await queuedCommand.Command.ExecuteAsync(queuedCommand.CancellationToken);
                queuedCommand.Completion.TrySetResult(result);
            }
            catch (OperationCanceledException) when (queuedCommand.CancellationToken.IsCancellationRequested)
            {
                queuedCommand.Completion.TrySetCanceled(queuedCommand.CancellationToken);
            }
            catch (Exception exception)
            {
                queuedCommand.Completion.TrySetException(exception);
            }
        }
    }

    private sealed record QueuedCommand(
        NotificationCommand Command,
        CancellationToken CancellationToken,
        TaskCompletionSource<NotificationResult> Completion);
}
