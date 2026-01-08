using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AvaloniaERP.Core.Entity
{
    public class Product : PersistentBase
    {
        public string Name { get; set; }

        public decimal PricePerUnit { get; set; }

        public double Weight { get; set; }

        public bool IsAvailable { get; set; }
    }

    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> b)
        {
            b.ToTable(nameof(Product));
            b.HasKey(p => p.Id);

            b.Property(x => x.Name).IsRequired().HasMaxLength(100);
        }
    }
}
