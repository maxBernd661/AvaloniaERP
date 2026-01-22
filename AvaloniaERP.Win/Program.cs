using Avalonia;
using AvaloniaERP.Win.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using AvaloniaERP.Core;
using AvaloniaERP.Win.Services;
using AvaloniaERP.Win.ViewModels.Base;
using AvaloniaERP.Win.ViewModels.EntitySpecific;
using AvaloniaERP.Win.Views.List;
using Microsoft.EntityFrameworkCore.Metadata;
using MainWindowViewModel = AvaloniaERP.Win.ViewModels.Base.MainWindowViewModel;

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

                    services.AddTransient<ProductListView>();


                    services.AddSingleton<IViewModelFactory, ViewModelFactory>();


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
