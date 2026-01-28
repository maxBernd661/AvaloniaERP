using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels.EntitySpecific;
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
        if (DataContext is OrderListViewModel vm &&
            sender is ListBox { SelectedItem: OrderRow row })
        {
            vm.SelectedRow = row;
            vm.OpenSelected.Execute(null);
        }
    }
}