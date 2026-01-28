using AvaloniaERP.Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AvaloniaERP.Core
{
    public class EntityContext(DbContextOptions<EntityContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Order>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<OrderItem>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Product>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntityContext).Assembly);
        }

        public override int SaveChanges()
        {
            UpdateTimestamp();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateTimestamp();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamp()
        {
            DateTime now = DateTime.Now;
            foreach (EntityEntry<PersistentBase> entry in ChangeTracker.Entries<PersistentBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreationTime = now;
                        entry.Entity.UpdateTime = now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdateTime = now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.UpdateTime = now;
                        entry.Entity.IsDeleted = true;
                        break;
                }
            }
        }
    }
}