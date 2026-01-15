using System.Collections.ObjectModel;
using System.Linq;

namespace AvaloniaERP.Win.ViewModels
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        private NavigationItem? selectedNavigationItem;
        private ViewModelBase? currentViewModel;

        public MainWindowViewModel(ProductListViewModel productListViewModel)
        {
            NavigationItems = new ObservableCollection<NavigationItem>
            {
                new("Dashboard", new DashboardHomeViewModel()),
                new("Products", productListViewModel)
            };

            SelectedNavigationItem = NavigationItems.First();
        }

        public ObservableCollection<NavigationItem> NavigationItems { get; }

        public NavigationItem? SelectedNavigationItem
        {
            get => selectedNavigationItem;
            set
            {
                if (SetProperty(ref selectedNavigationItem, value))
                {
                    CurrentViewModel = selectedNavigationItem?.ViewModel;
                }
            }
        }

        public ViewModelBase? CurrentViewModel
        {
            get => currentViewModel;
            private set => SetProperty(ref currentViewModel, value);
        }
    }

    public sealed record NavigationItem(string Title, ViewModelBase ViewModel);
}
