using Avalonia.Controls;
using AvaloniaERP.Win.ViewModels;
using ProductDetailViewModel = AvaloniaERP.Win.ViewModels.Detail.ProductDetailViewModel;

namespace AvaloniaERP.Win.Views.Detail;

public partial class ProductDetailView : UserControl
{
    public ProductDetailView()
    {
        InitializeComponent();
    }

    public ProductDetailView(ProductDetailViewModel vm) : this()
    {
        DataContext = vm;
    }
}