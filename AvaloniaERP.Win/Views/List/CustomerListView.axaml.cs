using Avalonia.Controls;
using AvaloniaERP.Win.ViewModels.EntitySpecific;

namespace AvaloniaERP.Win.Views.List;

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