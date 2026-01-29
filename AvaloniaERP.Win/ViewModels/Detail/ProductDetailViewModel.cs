using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaERP.Win.ViewModels.Detail;

public partial class ProductDetailViewModel : EntityDetailViewModel<Product>
{
    public ProductDetailViewModel(IServiceProvider sp) : base(sp)
    {
    }

    public ProductDetailViewModel(IServiceProvider sp, Product product) : base(sp, product)
    {
        Reset();
    }

    [ObservableProperty]
    [Required, MaxLength(100)]
    private string name = "";

    [ObservableProperty]
    [Range(typeof(decimal), "0", "99999")]
    private decimal pricePerUnit;

    [ObservableProperty]
    [Range(0, 99999)]
    private double weight;

    [ObservableProperty]
    private bool isAvailable;

    protected override sealed void Reset()
    {
        SetDefault();
        Name = Entity.Name;
        PricePerUnit = Entity.PricePerUnit;
        Weight = Entity.Weight;
        IsAvailable = Entity.IsAvailable;
    }

    protected override void Delete()
    {
        EntityContext context = ServiceProvider.GetRequiredService<EntityContext>();
        Product? existing = context.Set<Product>().FirstOrDefault(x => x.Id == EntityId);
        if (existing is null)
        {
            return;
        }

        context.Set<Product>().Remove(existing);

        context.SaveChanges();
        IListViewModel vm = ServiceProvider.GetRequiredService<IViewModelFactory>().CreateListView(typeof(Product));
        ServiceProvider.GetRequiredService<INavigationService>().Navigate(vm);

        ShowStatusMessage($"'{Entity.Name}' deleted.");
    }

    protected override void Write()
    {
        Entity.Name = Name;
        Entity.PricePerUnit = PricePerUnit;
        Entity.Weight = Weight;
        Entity.IsAvailable = IsAvailable;
    }
}