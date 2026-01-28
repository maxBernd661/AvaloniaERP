using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using AvaloniaERP.Win.ViewModels.Detail;
using AvaloniaERP.Win.ViewModels.EntitySpecific;
using Microsoft.Extensions.DependencyInjection;
using CustomerDetailViewModel = AvaloniaERP.Win.ViewModels.Detail.CustomerDetailViewModel;

namespace AvaloniaERP.Win.Views.Detail;

public partial class CustomerDetailView : UserControl
{
    public CustomerDetailView()
    {
        InitializeComponent();
    }

    public CustomerDetailView(CustomerDetailViewModel vm) : this()
    {
        DataContext = vm;
    }

    private void ListBox_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (sender is ListBox { SelectedItem: OrderRow row } && DataContext is CustomerDetailViewModel vm)
        {
            Order? item = vm.ServiceProvider.GetRequiredService<EntityContext>().Set<Order>().Find(row.Id);
            if (item is null)
            {
                return;
            }

            IDetailViewModel viewModel = vm.ServiceProvider.GetRequiredService<IViewModelFactory>()
                                          .CreateDetailView(typeof(Order), item);

            vm.ServiceProvider.GetRequiredService<INavigationService>().Navigate(viewModel);
        }
    }
}