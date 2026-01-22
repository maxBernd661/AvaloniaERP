using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaERP.Win.ViewModels.Base;

public interface INavigationService
{
    void Navigate(object? viewModel);
}

public sealed class NavigationService(MainWindowViewModel main)
    : INavigationService
{
    public void Navigate(object? viewModel) => main.Current = viewModel;
}