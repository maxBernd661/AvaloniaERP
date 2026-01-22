using System;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels.Base;
using CommunityToolkit.Mvvm.Input;
using System.Linq;

namespace AvaloniaERP.Win.ViewModels.EntitySpecific
{
    public partial class CustomerListViewModel : ListViewModelBase<Customer, CustomerRow>
    {
        private readonly IServiceProvider sp;
        private readonly INavigationService nav;

        public CustomerListViewModel(EntityContext entityContext, IServiceProvider sp, INavigationService nav)
            : base(entityContext)
        {
            this.sp = sp;
            this.nav = nav;
        }

        [RelayCommand]
        private void AddCustomer()
        {
            var detailVm = new CustomerDetailViewModel(sp);
            nav.Navigate(detailVm);
        }

        protected override IQueryable<Customer> ApplyFilter(IQueryable<Customer> q, string? filter)
            => q.Where(x => x.Name == filter);

        protected override IOrderedQueryable<Customer> ApplyOrder(IQueryable<Customer> q)
            => q.OrderBy(x => x.Name);

        protected override IQueryable<CustomerRow> Project(IQueryable<Customer> q)
            => q.Select(x => new CustomerRow(x));
    }
}