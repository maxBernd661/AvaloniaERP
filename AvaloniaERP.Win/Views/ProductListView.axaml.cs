using Avalonia.Controls;
using AvaloniaERP.Win.ViewModels;

namespace AvaloniaERP.Win.Views;

public partial class ProductListView : UserControl
{
    public ProductListView()
    {
        InitializeComponent();
    }

    public ProductListView(ProductListViewModel vm) : this()
    {
        DataContext = vm;
    }

}