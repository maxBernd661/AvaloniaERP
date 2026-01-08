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
}
