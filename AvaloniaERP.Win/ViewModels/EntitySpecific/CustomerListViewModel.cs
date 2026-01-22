using System;
using System.Linq;
using System.Windows.Input;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using AvaloniaERP.Win.ViewModels.Base;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaERP.Win.ViewModels.EntitySpecific
{
    public class CustomerListViewModel : ListViewModelBase<Customer, CustomerRow>
    {
        public ICommand OpenCustomerCommand { get; }

        public CustomerListViewModel(IServiceProvider sp) : base(sp)
        {
            OpenCustomerCommand = new RelayCommand(ShowCustomer);
        }

        private void ShowCustomer()
        {
            IDetailViewModel view = ServiceProvider.GetRequiredService<IViewModelFactory>().CreateDetailView(typeof(Customer));
            ServiceProvider.GetRequiredService<INavigationService>().Navigate(view);
        }

        protected override IQueryable<Customer> ApplyFilter(IQueryable<Customer> q, string? filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return q;
            }

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
