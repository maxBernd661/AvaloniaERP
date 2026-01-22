 using System;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels;
using AvaloniaERP.Win.ViewModels.EntitySpecific;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaERP.Win.Services
{
    public enum ViewKind
    {
        ListView,
        DetailView
    }

    public interface IViewModelFactory
    {
        IViewModel Create(Type entityType, ViewKind kind);

        IListViewModel CreateListView(Type entityType);

        IDetailViewModel CreateDetailView(Type entityType, PersistentBase? existing = null);
    }

    public interface IViewModel { }

    public interface IListViewModel : IViewModel { }


    public sealed class ViewModelFactory(IServiceProvider provider) : IViewModelFactory
    {
        private readonly IServiceProvider sp = provider;

        public IViewModel Create(Type entityType, ViewKind kind)
        {
            return kind == ViewKind.ListView ? CreateListView(entityType) : CreateDetailView(entityType);
        }

        public IListViewModel CreateListView(Type entityType)
        {
            Type? modelType = entityType switch
            {
                _ when entityType == typeof(Customer) => typeof(CustomerListViewModel),
                _ when entityType == typeof(Order) => typeof(OrderListViewModel),
                _ when entityType == typeof(Product) => typeof(ProductListViewModel),
                _ => throw new ArgumentException(nameof(entityType))
            };

            return (IListViewModel)sp.GetRequiredService(modelType);
        }

        public IDetailViewModel CreateDetailView(Type entityType, PersistentBase? existing = null)
        {
            IDetailViewModel vm = entityType switch
            {
                _ when entityType == typeof(Product) => ActivatorUtilities.CreateInstance<ProductDetailViewModel>(sp, existing!),
                _ => throw new ArgumentException(nameof(entityType))
            };

            return vm;
        }
    }
}
