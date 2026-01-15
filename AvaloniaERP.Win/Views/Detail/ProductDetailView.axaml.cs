using Avalonia.Controls;
using AvaloniaERP.Win.ViewModels;

namespace AvaloniaERP.Win.Views.Detail;

public partial class ProductDetailView : UserControl
{
    public ProductDetailView()
    {
        InitializeComponent();
    }

    public ProductDetailView(ProductDetailViewModel vm)
    {
        DataContext = vm;
    }
}