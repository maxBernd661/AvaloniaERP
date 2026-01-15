using Avalonia.Controls;
using AvaloniaERP.Win.ViewModels;

namespace AvaloniaERP.Win.Views;

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