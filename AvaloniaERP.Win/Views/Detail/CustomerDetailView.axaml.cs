using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaERP.Win.ViewModels;

namespace AvaloniaERP.Win;

public partial class CustomerDetailView : UserControl
{
    public CustomerDetailView()
    {
        InitializeComponent();
    }

    public CustomerDetailView(CustomerDetailViewModel vm)
    {
        DataContext = vm;
    }
}