using System;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaERP.Win.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly Action<ViewModelBase> _navigate;
        private readonly Func<ProductListViewModel> _productListFactory;

        public DashboardViewModel(Action<ViewModelBase> navigate, Func<ProductListViewModel> productListFactory)
        {
            _navigate = navigate;
            _productListFactory = productListFactory;
            OpenProductListCommand = new RelayCommand(OpenProductList);
        }

        public IRelayCommand OpenProductListCommand { get; }

        private void OpenProductList()
        {
            _navigate(_productListFactory());
        }
    }
}
