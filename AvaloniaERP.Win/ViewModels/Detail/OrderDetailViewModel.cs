using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using AvaloniaERP.Win.ViewModels.EntitySpecific;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaERP.Win.ViewModels.Detail
{
    public partial class OrderDetailViewModel : EntityDetailViewModel<Order>
    {
        public OrderItemListViewModel ItemListViewModel { get; }

        public ICommand OpenCustomerCommand { get; }

        public ObservableCollection<OrderItemRow> Items { get; } = [];

        public OrderDetailViewModel(IServiceProvider sp) : base(sp)
        {
            ItemListViewModel = new OrderItemListViewModel(sp);
            OpenCustomerCommand = new RelayCommand(OpenCustomer, () => Customer is not null);
            _ = LoadCustomers();
        }

        public OrderDetailViewModel(IServiceProvider sp, Order? entity) : base(sp, entity)
        {
            ItemListViewModel = new OrderItemListViewModel(sp);
            OpenCustomerCommand = new RelayCommand(OpenCustomer, () => Customer is not null);
            Reset();
            _ = LoadCustomers();
        }

        [ObservableProperty]
        [Required]
        private Customer? customer;

        [ObservableProperty]
        private OrderStatus status;

        public ObservableCollection<Customer> Customers { get; } = [];

        private void OpenCustomer()
        {
            if (Customer is null)
                return;

            IViewModelFactory factory = ServiceProvider.GetRequiredService<IViewModelFactory>();
            INavigationService nav = ServiceProvider.GetRequiredService<INavigationService>();

            IDetailViewModel view = factory.CreateDetailView(typeof(Customer));
            nav.Navigate(view);
        }

        protected override sealed void Reset()
        {
            Items.Clear();

            Customer = Entity.Customer;
            Status = Entity.Status;
            SyncSelectedCustomer();

            foreach (OrderItem item in Entity.Items)
            {
                Items.Add(new OrderItemRow(item));
            }
        }

        protected override void Write()
        {
            if (Customer is not null)
            {
                Entity.Customer = Customer;
            }

            Entity.Status = Status;
        }

        protected override void Delete()
        {
            EntityContext context = ServiceProvider.GetRequiredService<EntityContext>();
            Order? existing = context.Set<Order>().FirstOrDefault(x => x.Id == EntityId);
            if (existing is null)
            {
                return;
            }

            context.Set<Order>().Remove(existing);

            context.SaveChanges();
            IListViewModel vm = ServiceProvider.GetRequiredService<IViewModelFactory>().CreateListView(typeof(Order));
            ServiceProvider.GetRequiredService<INavigationService>().Navigate(vm);
        }

        private async Task LoadCustomers()
        {
            try
            {
                EntityContext context = ServiceProvider.GetRequiredService<EntityContext>();
                List<Customer> customers = await context.Customers.AsNoTracking()
                                                        .OrderBy(x => x.Name)
                                                        .ToListAsync();

                Customers.Clear();
                foreach (Customer customer in customers)
                {
                    Customers.Add(customer);
                }

                SyncSelectedCustomer();
            }
            catch (Exception ex)
            {
                //todo
            }
        }

        private void SyncSelectedCustomer()
        {
            if (Customer is null)
            {
                return;
            }

            Customer? match = Customers.FirstOrDefault(x => x.Id == Customer.Id);
            if (match is not null && !ReferenceEquals(match, Customer))
            {
                Customer = match;
            }
        }
    }
}