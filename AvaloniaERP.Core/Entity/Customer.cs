using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AvaloniaERP.Core.Entity
{
    public class Customer : PersistentBase
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }

        private readonly List<Order> orders = [];

        public IReadOnlyCollection<Order> Orders
        {
            get { return orders; }
        }
    }

    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> b)
        {
            b.ToTable(nameof(Customer));
            b.HasKey(x => x.Id);

            b.Property(x => x.Name).IsRequired().HasMaxLength(100);

            b.Navigation(x => x.Orders).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    public class CustomerRow(string name, string email, string phone, string address, bool isActive) : RowBase<Customer>
    {
        public CustomerRow(Customer customer) : this(customer.Name, customer.Email, customer.Phone, customer.Address,
            customer.IsActive){}

        public string Name { get; set; } = name;

        public string Email { get; set; } = email;

        public string Phone { get; set; } = phone;

        public string Address { get; set; } = address;

        public string IsActive { get; set; } = isActive ? "Active" : "Inactive";

        public override string AsString()
        {
            return $"{Name} ({Email}) - {IsActive}";
        }
    }
}
