using System.ComponentModel;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaERP.Win.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IViewModelFactory factory = null!;

        public MainWindowViewModel()
        {
        }

        public MainWindowViewModel(IViewModelFactory factory)
        {
            this.factory = factory;
           // Current = factory.Create<Product>(ViewKind.ListView);
        }

        private object? current;

        public object? Current
        {
            get { return current; }
            set { SetField(ref current, value); }

        }


        [RelayCommand]
        public void ShowProducts()
        {
            IViewModel vm = factory.Create(typeof(Product), ViewKind.ListView);
            Current = vm;
        }
        
        [RelayCommand]
        public void ShowCustomers()
        {
            IViewModel vm = factory.Create(typeof(Customer), ViewKind.ListView);
            Current = vm;
        }

        [RelayCommand]
        public void ShowOrders()
        {
            IViewModel vm = factory.Create(typeof(Order), ViewKind.ListView);
            Current = vm;
        }
    }
}
