using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaERP.Win.ViewModels;
using CustomerDetailViewModel = AvaloniaERP.Win.ViewModels.Detail.CustomerDetailViewModel;

namespace AvaloniaERP.Win.Views.Detail;

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