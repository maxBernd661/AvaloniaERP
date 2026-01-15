using Avalonia.Controls;
using AvaloniaERP.Win.ViewModels;

namespace AvaloniaERP.Win.Views;

public partial class CustomerListView : UserControl
{
    public CustomerListView()
    {
        InitializeComponent();
    }


    public CustomerListView(CustomerListViewModel vm) : this()
    {
        DataContext = vm;
    }

}