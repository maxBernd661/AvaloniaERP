using System.Linq;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels.Base;

namespace AvaloniaERP.Win.ViewModels.EntitySpecific
{
    public class ProductListViewModel(EntityContext entityContext) : ListViewModelBase<Product,ProductRow>(entityContext)
    {
        protected override IQueryable<Product> ApplyFilter(IQueryable<Product> q, string? filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return q;
            }

            return q.Where(x => x.Name == filter);
        }

        protected override IOrderedQueryable<Product> ApplyOrder(IQueryable<Product> q)
        {
            return q.OrderBy(x => x.Name);
        }

        protected override IQueryable<ProductRow> Project(IQueryable<Product> q)
        {
            return q.Select(x => new ProductRow(x));
        }
    }
}
