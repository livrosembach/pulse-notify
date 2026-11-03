namespace NotifyKit.Abstractions;
public interface IRetryBackoffPolicy { TimeSpan GetDelay(int retryNumber); }
