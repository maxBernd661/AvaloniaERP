using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaERP.Win.ViewModels.Detail
{
    public interface IDetailViewModel : IViewModel
    {
    }

    public abstract class DetailViewModelBase : ObservableValidator, IDetailViewModel
    {
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public abstract class EntityDetailViewModel<TEntity> : DetailViewModelBase where TEntity : PersistentBase
    {
        protected IServiceProvider ServiceProvider;
        protected TEntity Entity;

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ResetCommand { get; }

        protected EntityDetailViewModel(IServiceProvider sp, PersistentBase? entity = null)
        {
            ServiceProvider = sp;

            if (entity is null)
            {
                Entity = Activator.CreateInstance<TEntity>();
            }
            else
            {
                Entity = (TEntity)entity;
            }

            SaveCommand = new AsyncRelayCommand(Save);
            ResetCommand = new RelayCommand(Reset, CanReset);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
        }

        public Guid EntityId
        {
            get { return Entity?.Id ?? Guid.Empty; }
        }

        public string CreationTime
        {
            get { return Entity?.CreationTime.ToString("dd.MM.yyyy - hh:mm") ?? DateTime.MinValue.ToString("dd.MM.yyyy - hh:mm"); }
        }

        public string UpdateTime
        {
            get { return Entity?.UpdateTime.ToString("dd.MM.yyyy - hh:mm") ?? DateTime.MinValue.ToString("dd.MM.yyyy - hh:mm"); }
        }

        protected async Task Save()
        {
            Write();
            try
            {
                ValidateAllProperties();
            }
            catch (Exception ex)
            {
                //todo
            }

            if (HasErrors)
            {
                return;
            }

            Type serviceType = typeof(DataManipulationService<>).MakeGenericType(typeof(TEntity));
            IDataManipulationService service = (IDataManipulationService)ServiceProvider.GetRequiredService(serviceType);

            try
            {
                await service.SaveAsync(Entity);
            }
            catch (Exception ex)
            {
                //todo
            }
        }

        protected abstract void Write();

        protected void Validate(object value, [CallerMemberName] string? propName = null)
        {
            ValidateProperty(value, propName!);
        }

        protected virtual bool CanReset()
        {
            return true;
        }

        protected virtual bool CanDelete()
        {
            return true;
        }

        protected abstract void Reset();

        protected abstract void Delete();
    }
}