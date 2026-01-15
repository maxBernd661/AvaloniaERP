using System;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels;
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
        object Create(Type entityType, ViewKind kind);
        IViewModel Create<T>(ViewKind kind);
    }

    public interface IViewModel { }

    public interface IListViewModel : IViewModel { }


    public sealed class ViewModelFactory(IServiceProvider provider) : IViewModelFactory
    {
        private readonly IServiceProvider sp = provider;
        public object Create(Type entityType, ViewKind kind)
        {

            Type? modelType = entityType switch
            {
                _ when entityType == typeof(Customer) => typeof(CustomerListViewModel),
                _ when entityType == typeof(Order) => typeof(OrderListViewModel),
                _ when entityType == typeof(Product) => typeof(ProductListViewModel),
                _ => throw new ArgumentException(nameof(entityType))
            };

            return sp.GetRequiredService(modelType);
        }

        public IViewModel Create<T>(ViewKind kind)
        {
            return (IViewModel)Create(typeof(T), kind);
        }
    }
}
