using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels.EntitySpecific;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaERP.Win.ViewModels
{
    public partial class OrderDetailViewModel : EntityDetailViewModel<Order>
    {
        public ICommand OpenCustomerCommand;

        public OrderItemListViewModel ItemListViewModel { get; set; }

        public OrderDetailViewModel(IServiceProvider sp, PersistentBase? entity = null) : base(sp, entity)
        {
            ItemListViewModel = new OrderItemListViewModel(sp.GetRequiredService<EntityContext>());
            OpenCustomerCommand = new RelayCommand<Guid>(OpenCustomer);
        }

        [ObservableProperty]
        private Customer customer;

        [ObservableProperty]
        private OrderStatus status;

        public ObservableCollection<OrderItemRow> Items { get; }

        private void OpenCustomer(Guid id)
        {

        }

        protected override void Write()
        {
            Entity.Customer = Customer;
            Entity.Status = Status;

        }

        protected override void Reset()
        {
            Customer = Entity.Customer;
            Status = Entity.Status;
            foreach (OrderItem item in Entity.Items)
            {
                Items.Add(new OrderItemRow(item));
            }
        }

        protected override void Delete()
        {
            throw new NotImplementedException();
        }

        protected override void Cancel()
        {
            throw new NotImplementedException();
        }
    }
}
