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

        public void AddItem(OrderItem item)
        {
            if (items.Select(x => x.Id).Contains(item.Id))
            {
                items.Add(item);
            }
        }

        public void RemoveItem(Guid id)
        {
            if (items.FirstOrDefault(x => x.Id == id) is { } item)
            {
                items.Remove(item);
            }
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

    public class OrderQueryProfile : IQueryProfile<Order>
    {
        public IQueryable<Order> Apply(IQueryable<Order> query)
        {
            return query.Include(x => x.Customer).Include(x => x.Items).ThenInclude(x => x.Product);
        }
    }

    public class OrderMerger : IGraphMerger<Order>
    {
        public async Task Merge(EntityContext context, Order tracked, Order incoming, CancellationToken ct = default)
        {
            if (tracked.CustomerId != incoming.CustomerId)
            {
                Customer? customer = await context.Set<Customer>().FindAsync([incoming.CustomerId], ct);

                if (customer == null)
                {
                    return;
                }

                tracked.Customer =customer;
            }

            List<OrderItem> incomingItems = incoming.Items.ToList();
            List<OrderItem> trackedItems = tracked.Items.ToList();

            List<OrderItem> toRemove = trackedItems.Where(x => incomingItems.All(y => y.Id != x.Id)).ToList();

            foreach (OrderItem rem in toRemove)
            {
                tracked.RemoveItem(rem.Id);
                context.Remove(rem);
            }

            foreach (OrderItem item in incomingItems)
            {
                //item is new
                if (item.CreationTime == DateTime.MinValue &&
                    item.UpdateTime == DateTime.MinValue)
                {
                    item.OrderId = tracked.Id;
                    item.Order = tracked;

                    Product product = context.Set<Product>().Local.FirstOrDefault(p => p.Id == item.ProductId) ??
                                    await context.Set<Product>().FirstAsync(x => x.Id == item.ProductId, ct);

                    tracked.AddItem(item);
                    context.Entry(item).State = EntityState.Added;
                    continue;
                }

                OrderItem? toUpdate = trackedItems.FirstOrDefault(x => x.Id == item.Id);
                if (toUpdate == null)
                {
                    continue;
                }

                context.Entry(toUpdate).CurrentValues.SetValues(item);

                if (toUpdate.ProductId != item.ProductId)
                {
                    Product product = context.Set<Product>().Local.FirstOrDefault(p => p.Id == item.ProductId) ??
                                       await context.Set<Product>().FirstAsync(x => x.Id == item.ProductId, ct);

                    toUpdate.Product = product;
                    toUpdate.ProductId = item.ProductId;
                }
            }
        }
    }
}
