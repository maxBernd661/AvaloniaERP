using System;
using System.Linq;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;

namespace AvaloniaERP.Win.ViewModels
{
    public class CustomerListViewModel(EntityContext entityContext) : ListViewModelBase<Customer, CustomerRow>(entityContext)
    {
        protected override IQueryable<Customer> ApplyFilter(IQueryable<Customer> q, string? filter)
        {
            return q.Where(x => x.Name == filter);
        }

        protected override IOrderedQueryable<Customer> ApplyOrder(IQueryable<Customer> q)
        {
            return q.OrderBy(x => x.Name);
        }

        protected override IQueryable<CustomerRow> Project(IQueryable<Customer> q)
        {
            return q.Select(x => new CustomerRow(x));
        }
    }
}
