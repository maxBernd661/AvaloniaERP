using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AvaloniaERP.Core.Entity
{
    public class OrderItem : PersistentBase
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public Order Order { get; set; } = null!;

        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
    }

    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> b)
        {
            b.ToTable(nameof(OrderItem));
            b.HasKey(x => x.Id);

            b.HasIndex(x => new { x.OrderId, x.ProductId }).IsUnique();

            b.HasOne(x => x.Order)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class OrderItemRow(string order, string product, int quantity, decimal rowCost, double rowWeight)
        : RowBase<OrderItem>
    {
        public OrderItemRow(OrderItem orderItem) : this(orderItem.Order?.ToString() ?? string.Empty, orderItem.Product.Name, orderItem.Quantity, (orderItem.Product.PricePerUnit * orderItem.Quantity), (orderItem.Product.Weight * orderItem.Quantity))
        { }

        public string Order { get; set; } = order;

        public string Product { get; set; } = product;

        public int Quantity { get; set; } = quantity;

        public decimal RowCost { get; set; } = rowCost;

        public double RowWeight { get; set; } = rowWeight;

        public override string AsString()
        {
            return $"{Product} x {Quantity} - {RowCost:C}, {RowWeight:N}";
        }
    }
}
