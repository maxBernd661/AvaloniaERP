using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaERP.Win.ViewModels
{
    public abstract class ListViewModelBase<TEntity, TRow>(EntityContext entityContext) : ViewModelBase where TEntity: PersistentBase
    {
        private readonly EntityContext context = entityContext;

        private TRow? selectedItem;

        public TRow? SelectedRow
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        private string? filterString;

        public string FilterString
        {
            get => filterString ?? string.Empty;
            set => SetProperty(ref filterString, value);
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

        protected abstract IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> q, string? filter);
        protected abstract IOrderedQueryable<TEntity> ApplyOrder(IQueryable<TEntity> q);
        protected abstract IQueryable<TRow> Project(IQueryable<TEntity> q);
    }
}
