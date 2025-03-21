using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal class App : ApplicationContext
    {
        private readonly IHost _host;

        internal App()
        {
            _host = CreateHostBuilder().Build();


            IServiceProvider provider = _host.Services;

#if DEBUG
            IAppDbContextFactory factory = provider.GetService<IAppDbContextFactory>();
            using (AppDbContext context = factory.CreateDbContext())
            {
                context.Database.Migrate();
            }
#endif

            NavigationService<FileViewModel> navigationService
                = provider.GetRequiredService<NavigationService<FileViewModel>>();

            navigationService.Navigate();
            
            MainForm = provider.GetRequiredService<MainWindow>();
            MainForm.Show();

            _host.Start();
        }

        protected override void ExitThreadCore()
        {
            base.ExitThreadCore();

            _host.StopAsync();

            _host.Dispose(); // 释放所有获取的service
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .AddDbContext()
                .AddStores()
                .AddServices()
                .AddViewModels()
                .AddViews();
        }
    }
}
