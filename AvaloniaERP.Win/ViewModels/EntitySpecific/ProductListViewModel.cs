using System;
using System.Linq;
using System.Windows.Input;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using AvaloniaERP.Win.ViewModels.Base;
using AvaloniaERP.Win.ViewModels.Detail;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaERP.Win.ViewModels.EntitySpecific
{
    public class ProductListViewModel : ListViewModelBase<Product, ProductRow>
    {
        public ICommand OpenProductCommand { get; }

        public ProductListViewModel(IServiceProvider sp) : base(sp)
        {
            OpenProductCommand = new RelayCommand(ShowProduct);
            InitializeList();
        }

        private void ShowProduct()
        {
            IDetailViewModel view = ServiceProvider
                .GetRequiredService<IViewModelFactory>()
                .CreateDetailView(typeof(Product));

            ServiceProvider
                .GetRequiredService<INavigationService>()
                .Navigate(view);
        }

        protected override IQueryable<Product> ApplyFilter(IQueryable<Product> q, string? filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return q;
            }

            return q.Where(x => x.Name.Contains(filter));
        }

        protected override IOrderedQueryable<Product> ApplyOrder(IQueryable<Product> q)
        {
            return q.OrderBy(x => x.Name);
        }

        protected override IQueryable<ProductRow> Project(IQueryable<Product> q)
        {
            return q.Select(x => new ProductRow(x));
        }
    }
}