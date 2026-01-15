using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AvaloniaERP.Win.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}
