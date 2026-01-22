using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AvaloniaERP.Core.Entity
{
    public class Order : PersistentBase
    {
        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;

        public OrderStatus Status { get; set; }

        private readonly List<OrderItem> items = [];

        public IReadOnlyCollection<OrderItem> Items
        {
            get { return items; }
        }

    }

    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> b)
        {
            b.ToTable(nameof(Order));
            b.HasKey(x => x.Id);

            b.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

            b.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(x => x.Items)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            b.Navigation(x => x.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }

    public class OrderRow(string customer, OrderStatus status, decimal totalCost, double totalWeight)
        : RowBase<Order>
    {
        public OrderRow(Order order) : this(order.Customer.Name, order.Status,
            order.Items.Sum(x => x.Product.PricePerUnit), order.Items.Sum(x => x.Product.Weight)){}

        public string Customer { get; set; } = customer;

        public OrderStatus Status { get; set; } = status;

        public decimal TotalCost { get; set; } = totalCost;

        public double TotalWeight { get; set; } = totalWeight;

        public override string AsString()
        {
            return $"{Customer} - {Status.ToString()}, {TotalCost:C}, {TotalWeight:N}";
        }
    }

    public enum OrderStatus
    {
        None,
        Draft,
        Confirmed,
        Shipped,
        Cancelled
    }
}
