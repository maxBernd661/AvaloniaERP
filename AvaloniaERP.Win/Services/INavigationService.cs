using AvaloniaERP.Win.ViewModels.Base;

namespace AvaloniaERP.Win.Services;

public interface INavigationService
{
    void Navigate(IViewModel? viewModel);
}

public sealed class NavigationService(MainWindowViewModel main)
    : INavigationService
{
    public void Navigate(IViewModel? viewModel) => main.Current = viewModel;
}