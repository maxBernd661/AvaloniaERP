using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaERP.Win.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Func<ProductListViewModel> _productListFactory;

        public MainWindowViewModel(Func<ProductListViewModel> productListFactory)
        {
            _productListFactory = productListFactory;
            Dashboard = new DashboardViewModel(NavigateTo, _productListFactory);
            ShowDashboardCommand = new RelayCommand(ShowDashboard);
            CurrentView = Dashboard;
        }

        public DashboardViewModel Dashboard { get; }

        public IRelayCommand ShowDashboardCommand { get; }

        [ObservableProperty]
        private ViewModelBase? currentView;

        private void NavigateTo(ViewModelBase viewModel)
        {
            CurrentView = viewModel;
        }

        private void ShowDashboard()
        {
            CurrentView = Dashboard;
        }
    }
}
