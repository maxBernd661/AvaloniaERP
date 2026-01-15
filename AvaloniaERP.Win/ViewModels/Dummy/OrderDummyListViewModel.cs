using System.Collections.ObjectModel;
using AvaloniaERP.Core.Entity;

namespace AvaloniaERP.Win.ViewModels.Dummy
{
    public class OrderDummyListViewModel
    {
        public ObservableCollection<OrderRow> Items { get; } =
        [
            new("John Customer", OrderStatus.Draft, 120, 320.5d),
            new("Test Customington", OrderStatus.Confirmed, 3202, 300.05d)
        ];

        public OrderRow? SelectedRow { get; set; }
        public string FilterString { get; set; } = "Design-Filter";
    }
}
