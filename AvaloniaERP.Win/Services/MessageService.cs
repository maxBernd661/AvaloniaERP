using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvaloniaERP.Win.ViewModels.Base;

namespace AvaloniaERP.Win.Services;

public interface IMessageService
{
    void ShowMessage(string message, TimeSpan? duration = null);
}

public sealed class MessageService(MainWindowViewModel mainWindowViewModel) : IMessageService
{
    private static readonly TimeSpan DefaultDuration = TimeSpan.FromSeconds(4);
    private CancellationTokenSource? clearTokenSource;

    public void ShowMessage(string message, TimeSpan? duration = null)
    {
        clearTokenSource?.Cancel();
        clearTokenSource?.Dispose();
        clearTokenSource = new CancellationTokenSource();
        CancellationToken token = clearTokenSource.Token;
        string displayMessage = message ?? string.Empty;
        Dispatcher.UIThread.Post(() => mainWindowViewModel.ErrorText = displayMessage);
        _ = ClearAfterDelayAsync(duration ?? DefaultDuration, token);
    }

    private async Task ClearAfterDelayAsync(TimeSpan delay, CancellationToken token)
    {
        try
        {
            await Task.Delay(delay, token);
        }
        catch (TaskCanceledException)
        {
            return;
        }

        Dispatcher.UIThread.Post(() => mainWindowViewModel.ErrorText = string.Empty);
    }
}
