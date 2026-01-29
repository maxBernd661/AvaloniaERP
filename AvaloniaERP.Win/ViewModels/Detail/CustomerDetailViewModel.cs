using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using AvaloniaERP.Win.ViewModels.EntitySpecific;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvaloniaERP.Win.ViewModels.Detail;

public partial class CustomerDetailViewModel : EntityDetailViewModel<Customer>
{
    public OrderListViewModel OrdersViewModel { get; }

    public CustomerDetailViewModel(IServiceProvider sp) : base(sp)
    {
        OrdersViewModel = new OrderListViewModel(sp);
        OpenOrderCommand = new AsyncRelayCommand<OrderRow>(OpenOrder, row => row is not null);
    }

    public CustomerDetailViewModel(IServiceProvider sp, Customer? customer) : base(sp, customer)
    {
        OrdersViewModel = new OrderListViewModel(sp);
        OpenOrderCommand = new AsyncRelayCommand<OrderRow>(OpenOrder, row => row is not null);
        Reset();
    }

    public ICommand OpenOrderCommand { get; }

    public OrderRow? SelectedOrder { get; set; }

    [ObservableProperty]
    [Required, MaxLength(100)]
    private string name = "";

    [ObservableProperty]
    [Required, MaxLength(100)]
    private string email = "";

    [ObservableProperty]
    [Required, MaxLength(100)]
    private string phone = "";

    [ObservableProperty]
    [Required, MaxLength(100)]
    private string address = "";

    [ObservableProperty]
    private bool isActive;

    public ObservableCollection<OrderRow> Orders { get; } = [];

    private Task OpenOrder(OrderRow? row)
    {
        return Task.CompletedTask;
    }

    protected override sealed void Reset()
    {
        SetDefault();
        Name = Entity.Name;
        Email = Entity.Email;
        Address = Entity.Address;
        Phone = Entity.Phone;
        IsActive = Entity.IsActive;

        foreach (Order row in Entity.Orders)
        {
            Orders.Add(new OrderRow(row));
        }
    }

    protected override void Delete()
    {
        EntityContext context = ServiceProvider.GetRequiredService<EntityContext>();
        Customer? existing = context.Set<Customer>().FirstOrDefault(x => x.Id == EntityId);
        if (existing is null)
        {
            return;
        }

        context.Set<Customer>().Remove(existing);

        context.SaveChanges();
        IListViewModel vm = ServiceProvider.GetRequiredService<IViewModelFactory>().CreateListView(typeof(Customer));
        ServiceProvider.GetRequiredService<INavigationService>().Navigate(vm);

        ShowStatusMessage($"'{Entity.Name}' deleted.");
    }

    protected override void Write()
    {
        Entity.Name = Name;
        Entity.Email = Email;
        Entity.Address = Address;
        Entity.Phone = Phone;
        Entity.IsActive = IsActive;
    }
}