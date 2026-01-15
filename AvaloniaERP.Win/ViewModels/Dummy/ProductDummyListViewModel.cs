using System.Collections.ObjectModel;
using AvaloniaERP.Core.Entity;

namespace AvaloniaERP.Win.ViewModels.Dummy
{
    public class ProductDummyListViewModel
    {
        public ObservableCollection<ProductRow> Items { get; } =
        [
            new("Apfel",(decimal)0.99, 0.1, true),
            new("Banane",(decimal) 1.29, 0.1, false),
            new("Kaffee", (decimal)7.49, 0.5, true),
        ];

        public ProductRow? SelectedRow { get; set; }
        public string FilterString { get; set; } = "Design-Filter";
    }
}
