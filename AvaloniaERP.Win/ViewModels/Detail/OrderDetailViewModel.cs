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
        public IAsyncRelayCommand AddItemCommand { get; }
        public IRelayCommand RemoveItemCommand { get; }

        public ObservableCollection<OrderItemRow> Items { get; } = [];

        public OrderDetailViewModel(IServiceProvider sp) : base(sp)
        {
            ItemListViewModel = new OrderItemListViewModel(sp);
            OpenCustomerCommand = new RelayCommand(OpenCustomer, () => Customer is not null);
            AddItemCommand = new AsyncRelayCommand(AddItem);
            RemoveItemCommand = new RelayCommand(RemoveSelectedItem, () => SelectedItem is not null);
            _ = LoadCustomers();
            _ = LoadProducts();
        }

        public OrderDetailViewModel(IServiceProvider sp, Order? entity) : base(sp, entity)
        {
            ItemListViewModel = new OrderItemListViewModel(sp);
            OpenCustomerCommand = new RelayCommand(OpenCustomer, () => Customer is not null);
            AddItemCommand = new AsyncRelayCommand(AddItem);
            RemoveItemCommand = new RelayCommand(RemoveSelectedItem, () => SelectedItem is not null);
            _ = LoadCustomers();
            _ = LoadProducts();
            Reset();
        }

        [ObservableProperty]
        [Required]
        private Customer? customer;

        [ObservableProperty]
        private OrderStatus status;

        public ObservableCollection<Product> Products { get; } = [];

        [ObservableProperty]
        [Required]
        private Product? selectedProduct;

        [ObservableProperty]
        [Range(1, int.MaxValue)]
        private int selectedQuantity = 1;

        [ObservableProperty]
        private OrderItemRow? selectedItem;

        public ObservableCollection<Customer> Customers { get; } = [];

        private void OpenCustomer()
        {
            if (Customer is null)
            {
                return;
            }

            IViewModelFactory factory = ServiceProvider.GetRequiredService<IViewModelFactory>();
            INavigationService nav = ServiceProvider.GetRequiredService<INavigationService>();

            IDetailViewModel view = factory.CreateDetailView(typeof(Customer));
            nav.Navigate(view);
        }

        protected override sealed void Reset()
        {
            Items.Clear();

            SetDefault();
            Customer = Entity.Customer;
            Status = Entity.Status;
            SyncSelectedCustomer();

            foreach (OrderItem item in Entity.Items)
            {
                Items.Add(new OrderItemRow(item));
            }

            SelectedItem = Items.FirstOrDefault();
        }

        protected override void Write()
        {
            if (Customer is not null)
            {
                Entity.Customer = Customer;
            }

            Entity.Status = Status;
            Entity.CustomerId = Customer?.Id ?? Entity.CustomerId;
        }

        private Task AddItem()
        {
            ValidateProperty(SelectedProduct, nameof(SelectedProduct));
            ValidateProperty(SelectedQuantity, nameof(SelectedQuantity));

            if (HasErrors || SelectedProduct is null)
            {
                return Task.CompletedTask;
            }

            OrderItem? existing = Entity.Items.FirstOrDefault(x => x.ProductId == SelectedProduct.Id);
            if (existing is not null)
            {
                existing.Quantity += SelectedQuantity;
                OrderItemRow? row = Items.FirstOrDefault(x => x.Id == existing.Id);
                if (row is not null)
                {
                    int rowIndex = Items.IndexOf(row);
                    Items[rowIndex] = new OrderItemRow(existing);
                    SelectedItem = Items[rowIndex];
                }
                else
                {
                    Items.Add(new OrderItemRow(existing));
                    SelectedItem = Items.LastOrDefault();
                }

                return Task.CompletedTask;
            }

            OrderItem item = new()
            {
                ProductId = SelectedProduct.Id,
                Product = SelectedProduct,
                Quantity = SelectedQuantity,
                Order = Entity,
                OrderId = Entity.Id
            };

            Entity.AddItem(item);
            Items.Add(new OrderItemRow(item));
            SelectedItem = Items.LastOrDefault();

            return Task.CompletedTask;
        }

        private void RemoveSelectedItem()
        {
            if (SelectedItem is null)
            {
                return;
            }

            OrderItem? existing = Entity.Items.FirstOrDefault(x => x.Id == SelectedItem.Id);
            if (existing is not null)
            {
                Entity.RemoveItem(existing.Id);
            }

            Items.Remove(SelectedItem);
            SelectedItem = Items.FirstOrDefault();
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

        private async Task LoadProducts()
        {
            try
            {
                EntityContext context = ServiceProvider.GetRequiredService<EntityContext>();
                List<Product> products = await context.Products.AsNoTracking()
                                                      .OrderBy(x => x.Name)
                                                      .ToListAsync();

                Products.Clear();
                foreach (Product product in products)
                {
                    Products.Add(product);
                }

                if (SelectedProduct is null)
                {
                    SelectedProduct = Products.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                //todo
            }
        }
    }
}