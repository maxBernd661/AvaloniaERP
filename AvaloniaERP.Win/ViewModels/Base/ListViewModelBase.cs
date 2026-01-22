using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaERP.Win.ViewModels.Base
{
    public abstract class ListViewModelBase<TEntity, TRow>(IServiceProvider sp) : IListViewModel, INotifyPropertyChanged
        where TEntity : PersistentBase
        where TRow : IEntityRow
    {
        public readonly IServiceProvider ServiceProvider = sp;
        private readonly EntityContext context = sp.GetRequiredService<EntityContext>();

        public event PropertyChangedEventHandler? PropertyChanged;

        private TRow? selectedItem;

        public TRow? SelectedRow
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged();
            }
        }

        private string? filterString;

        public string? FilterString
        {
            get { return filterString; }
            set
            {
                filterString = value;
                OnPropertyChanged();
            }
        }

        public async Task ReloadAsync()
        {
            Items.Clear();

            IQueryable<TEntity> set = context.Set<TEntity>().AsNoTracking();
            IQueryable<TEntity> query = ApplyFilter(set, FilterString);
            query = ApplyOrder(query);
            List<TRow> rows = await Project(query).ToListAsync();
            foreach (var row in rows)
            {
                Items.Add(row);
            }
        }

        public ObservableCollection<TRow> Items { get; } = [];

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }


        protected abstract IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> q, string? filter);
        protected abstract IOrderedQueryable<TEntity> ApplyOrder(IQueryable<TEntity> q);
        protected abstract IQueryable<TRow> Project(IQueryable<TEntity> q);
    }


}
