using System.Collections.ObjectModel;
using AvaloniaERP.Core.Entity;

namespace AvaloniaERP.Win.ViewModels
{
    public class CustomerDummyListViewModel
    {
        public ObservableCollection<CustomerRow> Items { get; } =
        [
            new("John Customer", "john@test.com", "+49 05527 99940", "Default Avenue 3, 37115 Duderstadt", true),
            new("Reginald the Inactive", "reg@ina.ld", "+01 123 45-55", "John Street 5", false)
        ];

        public CustomerRow? SelectedRow { get; set; }
        public string FilterString { get; set; } = "Design-Filter";
    }
}
