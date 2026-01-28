using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaERP.Win.ViewModels;

namespace AvaloniaERP.Win.Views.Detail;

public partial class OrderDetailView : UserControl
{
    public OrderDetailView()
    {
        InitializeComponent();
    }

    public OrderDetailView(OrderDetailViewModel vm)
    {
        DataContext = vm;
    }
}