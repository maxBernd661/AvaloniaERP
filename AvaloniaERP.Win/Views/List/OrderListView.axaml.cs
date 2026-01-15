using Avalonia.Controls;
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
}