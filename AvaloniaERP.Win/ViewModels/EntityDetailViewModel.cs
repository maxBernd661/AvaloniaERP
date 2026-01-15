using AvaloniaERP.Core.Entity;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace AvaloniaERP.Win.ViewModels
{
    public interface IDetailViewModel
    {

    }

    public abstract class DetailViewModelBase : ObservableValidator,  IDetailViewModel
    {
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    public abstract class EntityDetailViewModel<TEntity>(IServiceProvider sp, PersistentBase entity) : DetailViewModelBase where TEntity : PersistentBase
    {
        protected IServiceProvider ServiceProvider = sp;
        protected TEntity Entity = (TEntity)entity;

        public Guid EntityId
        {
            get { return Entity.Id; }
        }

        protected abstract void Reset();

        protected void Save()
        {
            Write();
            ValidateAllProperties();
        }

        protected abstract void Write();

        protected void Validate(object value, [CallerMemberName] string? propName = null)
        {
            ValidateProperty(value, propName!);
        }
    }
}
 