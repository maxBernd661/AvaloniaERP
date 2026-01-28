using AvaloniaERP.Core.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaERP.Core.Test
{
    public class EntityContextTests
    {
        [Fact]
        public async Task SaveChangesAsyncTest()
        {
            SQLitePCL.Batteries_V2.Init();
            await using SqliteConnection connection = new("Filename=:memory:");
            await connection.OpenAsync();

            DbContextOptions<EntityContext> options = new DbContextOptionsBuilder<EntityContext>()
                                                     .UseSqlite(connection)
                                                     .Options;

            await using EntityContext context = new(options);
            await context.Database.EnsureCreatedAsync();

            Product product = new()
            {
                Name = "Test Product",
                PricePerUnit = 1.25m,
                Weight = 2.0,
                IsAvailable = true
            };

            DateTime beforeAdd = DateTime.Now;
            context.Products.Add(product);
            await context.SaveChangesAsync();
            DateTime afterAdd = DateTime.Now;

            Assert.InRange(product.CreationTime, beforeAdd, afterAdd);
            Assert.InRange(product.UpdateTime, beforeAdd, afterAdd);
            Assert.Equal(product.CreationTime, product.UpdateTime);

            DateTime originalCreation = product.CreationTime;
            DateTime originalUpdate = product.UpdateTime;

            await Task.Delay(10);
            product.Name = "Updated Product";
            await context.SaveChangesAsync();

            Assert.Equal(originalCreation, product.CreationTime);
            Assert.True(product.UpdateTime > originalUpdate);

            DateTime updateAfterModify = product.UpdateTime;

            await Task.Delay(10);
            context.Products.Remove(product);
            await context.SaveChangesAsync();

            Assert.True(product.IsDeleted);
            Assert.True(product.UpdateTime > updateAfterModify);

            int filteredCount = await context.Products.CountAsync();
            Assert.Equal(0, filteredCount);

            Product deleted = await context.Products.IgnoreQueryFilters().SingleAsync();
            Assert.True(deleted.IsDeleted);
        }
    }
}