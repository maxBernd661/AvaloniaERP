using Avalonia;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using AvaloniaERP.Core;
using AvaloniaERP.Core.Entity;
using AvaloniaERP.Win.Services;
using AvaloniaERP.Win.ViewModels.EntitySpecific;
using CustomerDetailViewModel = AvaloniaERP.Win.ViewModels.Detail.CustomerDetailViewModel;
using MainWindowViewModel = AvaloniaERP.Win.ViewModels.Base.MainWindowViewModel;
using OrderDetailViewModel = AvaloniaERP.Win.ViewModels.Detail.OrderDetailViewModel;
using ProductDetailViewModel = AvaloniaERP.Win.ViewModels.Detail.ProductDetailViewModel;

namespace AvaloniaERP.Win
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            SQLitePCL.Batteries_V2.Init();
            AppHost = CreateHostBuilder(args).Build();
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static IHost AppHost { get; private set; } = null!;

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            //avalonia unterstützt generic host nicht, einmal vorher bauen und dann in die app reingeben, passt
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                }).ConfigureServices((context, services) =>
                {
                    services.AddDbContext<EntityContext>(options =>
                    {
                        options.UseSqlite(context.Configuration.GetConnectionString("Default"));
                    });

                    services.AddSingleton<MainWindowViewModel>();
                    services.AddSingleton<INavigationService, NavigationService>();

                    services.AddTransient<CustomerListViewModel>();
                    services.AddTransient<OrderListViewModel>();
                    services.AddTransient<ProductListViewModel>();

                    services.AddTransient<CustomerDetailViewModel>();
                    services.AddTransient<ProductDetailViewModel>();
                    services.AddTransient<OrderDetailViewModel>();

                    services.AddTransient<IQueryProfile<Product>, ProductQueryProfile>();
                    services.AddTransient<IQueryProfile<Customer>, CustomerQueryProfile>();
                    services.AddTransient<IQueryProfile<Order>, OrderQueryProfile>();
                    services.AddTransient<IQueryProfile<OrderItem>, OrderItemQueryProfile>();

                    services.AddTransient<IGraphMerger<Order>, OrderMerger>();

                    services.AddSingleton<IViewModelFactory, ViewModelFactory>();
                    services.AddTransient(typeof(DataManipulationService<>));


                });
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
        }
    }
}
