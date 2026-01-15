using Avalonia.Controls;
using AvaloniaERP.Win.ViewModels.EntitySpecific;

namespace AvaloniaERP.Win.Views.List;

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