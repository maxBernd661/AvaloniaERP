using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels.Base;
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

    private void ListBox_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataContext is ProductListViewModel vm &&
            sender is ListBox { SelectedItem: ProductRow row })
        {
            vm.SelectedRow = row;
            vm.OpenSelected.Execute(null);
        }
    }
}