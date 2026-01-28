using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using AvaloniaERP.Win.ViewModels.EntitySpecific;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        }

        public OrderDetailViewModel(IServiceProvider sp, Order? entity) : base(sp, entity)
        {
            ItemListViewModel = new OrderItemListViewModel(sp);
            OpenCustomerCommand = new RelayCommand(OpenCustomer, () => Customer is not null);
            Reset();
        }

        [ObservableProperty]
        [Required]
        private Customer? customer;

        [ObservableProperty]
        private OrderStatus status;

        private void OpenCustomer()
        {
            if (Customer is null)
                return;

            var factory = ServiceProvider.GetRequiredService<IViewModelFactory>();
            var nav = ServiceProvider.GetRequiredService<INavigationService>();

            var view = factory.CreateDetailView(typeof(Customer));
            nav.Navigate(view);
        }

        protected override sealed void Reset()
        {
            Items.Clear();

            Customer = Entity.Customer;
            Status = Entity.Status;

            foreach (var item in Entity.Items)
            {
                Items.Add(new OrderItemRow(item));
            }
        }

        protected override void Write()
        {
            if (Customer is not null)
                Entity.Customer = Customer;

            Entity.Status = Status;
        }

        protected override void Delete() => throw new NotImplementedException();
    }
}