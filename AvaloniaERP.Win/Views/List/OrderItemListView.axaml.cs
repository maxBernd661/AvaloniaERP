using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels.EntitySpecific;

namespace AvaloniaERP.Win.Views.List;

public partial class OrderItemListView : UserControl
{
    public OrderItemListView()
    {
        InitializeComponent();
    }

    public OrderItemListView(OrderItemListViewModel vm) : this()
    {
        DataContext = vm;
    }

    private void InputElement_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is OrderItemListViewModel vm &&
            sender is Control { DataContext: OrderItemRow row })
        {
            vm.SelectedRow = row;
            vm.OpenSelected.Execute(null);
        }
    }
}