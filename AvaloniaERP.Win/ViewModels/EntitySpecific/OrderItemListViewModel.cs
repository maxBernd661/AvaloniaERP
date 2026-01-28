using System;
using System.Linq;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels.Base;

namespace AvaloniaERP.Win.ViewModels.EntitySpecific
{
    public class OrderItemListViewModel : ListViewModelBase<OrderItem, OrderItemRow>
    {
        public OrderItemListViewModel(IServiceProvider sp) : base(sp)
        {
            InitializeList();
        }

        protected override IQueryable<OrderItem> ApplyFilter(IQueryable<OrderItem> q, string? filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return q;
            }

            return q.Where(x => x.Product.Name.Contains(filter));
        }

        protected override IOrderedQueryable<OrderItem> ApplyOrder(IQueryable<OrderItem> q)
        {
            return q.OrderBy(x => x.Product);
        }

        protected override IQueryable<OrderItemRow> Project(IQueryable<OrderItem> q)
        {
            return q.Select(x => new OrderItemRow(x));
        }
    }
}
