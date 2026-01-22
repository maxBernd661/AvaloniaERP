using Avalonia.Controls;
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

}