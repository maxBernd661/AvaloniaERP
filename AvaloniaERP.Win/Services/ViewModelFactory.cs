using System;
using System.Linq;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaERP.Win.Services
{
    public class ListViewModel<T>(EntityContext entityContext) : ListViewModelBase<T, IEntityRow>(entityContext), IViewModel where T : PersistentBase
    {
        protected override IQueryable<T> ApplyFilter(IQueryable<T> q, string? filter)
        {
            return q;
        }

        protected override IOrderedQueryable<T> ApplyOrder(IQueryable<T> q)
        {
            return q.OrderBy(x => x);
        }

        protected override IQueryable<IEntityRow> Project(IQueryable<T> q)
        {
            return q.Select(x => (IEntityRow)x);
        }
    }

    public class DetailViewModel<T>  where T : PersistentBase{}

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

    public interface IViewModel
    {
    }

    public sealed class ViewModelFactory(IServiceProvider provider) : IViewModelFactory
    {
        private readonly IServiceProvider sp = provider;
        public object Create(Type entityType, ViewKind kind)
        {
            Type open = kind switch
            {
                ViewKind.ListView => typeof(ListViewModel<>),
                ViewKind.DetailView => typeof(DetailViewModel<>),
                _ => throw new ArgumentException(nameof(kind))
            };

            Type closed = open.MakeGenericType(entityType);
            return sp.GetRequiredService(closed);
        }

        public IViewModel Create<T>(ViewKind kind)
        {
            return (IViewModel)Create(typeof(T), kind);
        }
    }
}
