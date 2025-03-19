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

            _host.Start();

            IServiceProvider provider = _host.Services;

            NavigationService<FileViewModel> navigationService
                = provider.GetRequiredService<NavigationService<FileViewModel>>();

            navigationService.Navigate();

            MainForm = provider.GetRequiredService<MainView>();
            MainForm.Show();
        }

        protected override void ExitThreadCore()
        {
            base.ExitThreadCore();

            _host.StopAsync();

            _host.Dispose();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .AddStores()
                .AddServices()
                .AddViewModels()
                .AddViews();
        }
    }
}
