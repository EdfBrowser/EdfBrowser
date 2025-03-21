using EdfBrowser.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace EdfBrowser.App
{
    internal static class ViewModelExtension
    {
        internal static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                services.AddSingleton<MainViewModel>();

                services.AddSingleton(s => CreateEdfPlotNavigationService(s));
                services.AddSingleton(s => CreateFileViewNavigationService(s));
                services.AddSingleton(s => CreateSignalListNavigationService(s));

                services.AddTransient<SignalListViewModel>();
                services.AddTransient<FileViewModel>();

                services.AddTransient<EdfPlotViewModel>();

                services.AddTransient<PlotViewModel>();
                services.AddTransient<TimelineViewModel>();
            });
        }

        private static NavigationService<FileViewModel> CreateFileViewNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<FileViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<FileViewModel>());
        }

        private static NavigationService<SignalListViewModel> CreateSignalListNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<SignalListViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<SignalListViewModel>());
        }

        private static NavigationService<EdfPlotViewModel> CreateEdfPlotNavigationService(IServiceProvider serviceProvider)
        {
            return new NavigationService<EdfPlotViewModel>(
                serviceProvider.GetRequiredService<NavigationStore>(),
                () => serviceProvider.GetRequiredService<EdfPlotViewModel>());
        }
    }
}
