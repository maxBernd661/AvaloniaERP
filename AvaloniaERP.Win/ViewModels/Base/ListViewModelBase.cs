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
using AvaloniaERP.Win.ViewModels.Detail;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaERP.Win.ViewModels.Base
{
    public abstract class ListViewModelBase<TEntity, TRow> : IListViewModel, INotifyPropertyChanged
        where TEntity : PersistentBase
        where TRow : IEntityRow
    {
        public readonly IServiceProvider ServiceProvider;
        private readonly EntityContext context;

        public event PropertyChangedEventHandler? PropertyChanged;

        public IAsyncRelayCommand OpenSelected { get; }
        public IAsyncRelayCommand DeleteSelected { get; }

        protected void InitializeList()
        {
            PropertyChanged += (o, args) =>
            {
                if (args.PropertyName == nameof(FilterString))
                {
                    _ = ReloadAsync();
                }
            };

            _ = ReloadAsync();
        }

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

        protected ListViewModelBase(IServiceProvider sp)
        {
            ServiceProvider = sp;
            context = sp.GetRequiredService<EntityContext>();
            OpenSelected = new AsyncRelayCommand(ShowSelectedAsync);
            DeleteSelected = new AsyncRelayCommand(DeleteSelectedAsync, CanDelete);
        }

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
            try
            {
                Items.Clear();

                IQueryable<TEntity> set = context.Set<TEntity>();
                IQueryProfile<TEntity> queryProfile = ServiceProvider.GetRequiredService<IQueryProfile<TEntity>>();
                set = queryProfile.Apply(set);
                IQueryable<TEntity> query = ApplyFilter(set, FilterString);
                query = ApplyOrder(query);
                List<TRow> rows = await Project(query).ToListAsync();
                foreach (TRow row in rows)
                {
                    Items.Add(row);
                }
            }
            catch (Exception ex)
            {
                //
            }
        }

        public async Task<TEntity?> GetEntity(Guid id)
        {
            IQueryProfile<TEntity> queryProfile = ServiceProvider.GetRequiredService<IQueryProfile<TEntity>>();
            IQueryable<TEntity> query = queryProfile.Apply(context.Set<TEntity>());
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        protected async Task ShowSelectedAsync()
        {
            if (SelectedRow is not RowBase<TEntity> row)
            {
                return;
            }

            TEntity? item = await GetEntity(row.Id);
            if (item is null)
            {
                return;
            }

            IDetailViewModel viewModel = ServiceProvider.GetRequiredService<IViewModelFactory>()
                                                        .CreateDetailView(typeof(TEntity), item);

            ServiceProvider.GetRequiredService<INavigationService>().Navigate(viewModel);
        }

        protected async Task DeleteSelectedAsync()
        {
            if (SelectedRow is not RowBase<TEntity> row)
            {
                return;
            }

            TEntity? item = await GetEntity(row.Id);
            if (item is null)
            {
                return;
            }

            context.Set<TEntity>().Remove(item);
            await context.SaveChangesAsync();

            await ReloadAsync();

            ShowStatusMessage($"{typeof(TEntity).Name} deleted");
        }

        public bool CanDelete()
        {
            return SelectedRow is not null;
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

        protected void ShowStatusMessage(string message)
        {
            ServiceProvider.GetRequiredService<IMessageService>().ShowMessage(message);
        }

    }
}