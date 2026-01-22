using System;
using System.Linq;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels.Base;

namespace AvaloniaERP.Win.ViewModels.EntitySpecific
{
    public class OrderListViewModel(IServiceProvider sp) : ListViewModelBase<Order, OrderRow>(sp)
    {
        protected override IQueryable<Order> ApplyFilter(IQueryable<Order> q, string? filter)
        {
            if(string.IsNullOrEmpty(filter))
            {
                return q;
            }

            return q.Where(x => x.Customer.Name == filter);
        }

        protected override IOrderedQueryable<Order> ApplyOrder(IQueryable<Order> q)
        {
            return q.OrderBy(x => x.Customer);
        }

        protected override IQueryable<OrderRow> Project(IQueryable<Order> q)
        {
            return q.Select(x => new OrderRow(x));
        }
    }
}
