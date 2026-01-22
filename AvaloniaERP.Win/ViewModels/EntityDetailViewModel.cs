using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvaloniaERP.Win.ViewModels
{
    public interface IDetailViewModel : IViewModel
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

    public abstract class EntityDetailViewModel<TEntity> : DetailViewModelBase where TEntity : PersistentBase
    {
        protected IServiceProvider ServiceProvider;
        protected TEntity? Entity;

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand CancelCommand { get; }

        protected EntityDetailViewModel(IServiceProvider sp, PersistentBase? entity = null)
        {
            ServiceProvider = sp;
            Entity = (TEntity?)entity;

            SaveCommand = new RelayCommand(Save, CanSave);
            ResetCommand = new RelayCommand(Reset, CanReset);
            CancelCommand = new RelayCommand(Cancel, CanCancel);
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

        protected virtual bool CanSave()
        {
            return true;
        }

        protected virtual bool CanReset()
        {
            return true;
        }

        protected virtual bool CanCancel()
        {
            return true;
        }

        protected virtual bool CanDelete()
        {
            return true;
        }

        protected abstract void Reset();

        protected abstract void Delete();

        protected abstract void Cancel();
    }

    public class AsyncRelayCommand(Func<Task> execute) : ICommand
    {
        public bool CanExecute(object? parameter) => true;

        public async void Execute(object? parameter)
        {
            await execute();
        }

        public event EventHandler? CanExecuteChanged;
    }
}
 