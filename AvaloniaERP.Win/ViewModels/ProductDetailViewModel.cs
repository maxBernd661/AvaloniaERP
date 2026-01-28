using AvaloniaERP.Core.Entity;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace AvaloniaERP.Win.ViewModels;

public partial class ProductDetailViewModel : EntityDetailViewModel<Product>
{
    public ProductDetailViewModel(IServiceProvider sp) : base(sp)
    {
    }

    public ProductDetailViewModel(IServiceProvider sp, Product? product) : base(sp, product)
    {
    }

    [ObservableProperty]
    [Required, MaxLength(100)]
    private string name = "";

    [ObservableProperty]
    [Range(typeof(decimal), "0.0", "99999")]
    private decimal pricePerUnit;

    [ObservableProperty]
    [Range(0, 99999)]
    private double weight;

    [ObservableProperty]
    private bool isAvailable;

    protected override void Reset()
    {
        Name = Entity.Name;
        PricePerUnit = Entity.PricePerUnit;
        Weight = Entity.Weight;
        IsAvailable = Entity.IsAvailable;
    
    }

    protected override void Delete()
    {
        throw new NotImplementedException();
    }

    protected override void Cancel()
    {
        throw new NotImplementedException();
    }

    protected override void Write()
    {
        Entity.Name = Name;
        Entity.PricePerUnit = PricePerUnit;
        Entity.Weight = Weight;
        Entity.IsAvailable = IsAvailable;
    }
}