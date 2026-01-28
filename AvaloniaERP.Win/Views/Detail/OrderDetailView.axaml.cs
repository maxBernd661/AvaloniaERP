using Avalonia.Controls;
using Avalonia.Input;
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
}