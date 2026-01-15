using System.Text;
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

    public class ProductRow(string name, decimal price, double weight, bool isAvailable) : RowBase<Product>
    {
        public ProductRow(Product product) : this(product.Name, product.PricePerUnit, product.Weight, product.IsAvailable){}

        public string Name { get; set; } = name;

        public string PricePerUnit { get; set; } = price.ToString("C");

        public string Weight { get; set; } = weight.ToString("N");

        public string IsAvailable { get; set; } = isAvailable ? "Available" : "Unavailable";

        public override string AsString()
        {
            return $"{Name} - {PricePerUnit} - {Weight}kg - {IsAvailable}";
        }
    }
}
