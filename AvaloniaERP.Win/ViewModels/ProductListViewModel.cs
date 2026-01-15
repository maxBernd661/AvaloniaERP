using System;
using System.Linq;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;

namespace AvaloniaERP.Win.ViewModels
{
    public class ProductListViewModel(EntityContext entityContext) : ListViewModelBase<Product,ProductRow>(entityContext)
    {
        protected override IQueryable<Product> ApplyFilter(IQueryable<Product> q, string? filter)
        {
            return q.Where(x => x.Name == filter);
        }

        protected override IOrderedQueryable<Product> ApplyOrder(IQueryable<Product> q)
        {
            return q.OrderBy(x => x.Name);
        }

        protected override IQueryable<ProductRow> Project(IQueryable<Product> q)
        {
            return q.Select(x => new ProductRow(x.Name, x.PricePerUnit, x.Weight, x.IsAvailable));
        }
    }
}
