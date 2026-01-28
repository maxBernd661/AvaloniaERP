using System;
using System.Linq;
using System.Windows.Input;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using AvaloniaERP.Win.ViewModels.Base;
using AvaloniaERP.Win.ViewModels.Detail;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaERP.Win.ViewModels.EntitySpecific
{
    public class OrderListViewModel : ListViewModelBase<Order, OrderRow>
    {
        public ICommand OpenOrderCommand { get; }

        public OrderListViewModel(IServiceProvider sp) : base(sp)
        {
            OpenOrderCommand = new RelayCommand(ShowOrder);
            InitializeList();
        }

        private void ShowOrder()
        {
            IDetailViewModel view = ServiceProvider
                .GetRequiredService<IViewModelFactory>()
                .CreateDetailView(typeof(Order));

            ServiceProvider
                .GetRequiredService<INavigationService>()
                .Navigate(view);
        }

        protected override IQueryable<Order> ApplyFilter(IQueryable<Order> q, string? filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return q;
            }

            return q.Where(x => x.Customer.Name.Contains(filter));
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