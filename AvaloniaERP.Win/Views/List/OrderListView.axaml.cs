using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels.EntitySpecific;

namespace AvaloniaERP.Win.Views.List;

public partial class OrderListView : UserControl
{
    public OrderListView()
    {
        InitializeComponent();
    }

    public OrderListView(OrderListViewModel vm) : this()
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