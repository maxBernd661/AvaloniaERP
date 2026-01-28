using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels.EntitySpecific;
using OrderDetailViewModel = AvaloniaERP.Win.ViewModels.Detail.OrderDetailViewModel;

namespace AvaloniaERP.Win.Views.Detail;

public partial class OrderDetailView : UserControl
{
    public OrderDetailView()
    {
        InitializeComponent();
    }

    public OrderDetailView(OrderDetailViewModel vm) : this()
    {
        DataContext = vm;
    }

    private void InputElement_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
    }

    private void ListBox_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is OrderItemListViewModel vm &&
            sender is ListBox { SelectedItem: OrderItemRow row })
        {
            vm.SelectedRow = row;
            vm.OpenSelected.Execute(null);
        }
    }
}