using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EdfBrowser.App
{
    internal static class ViewExtension
    {
        internal static IHostBuilder AddViews(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                services.AddSingleton<ViewFactory>();

                services.AddSingleton(s =>
                    new MainWindow(s.GetRequiredService<ViewFactory>())
                    { DataContext = s.GetRequiredService<MainViewModel>() });

                services.AddTransient<FileView>();
                services.AddTransient<SignalListView>();

                services.AddTransient<EdfPlotView>();

                services.AddTransient<PlotView>();
                services.AddTransient<TimelineView>();
            });
        }
    }
}
