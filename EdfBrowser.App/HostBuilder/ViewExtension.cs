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
                services.AddSingleton<MainView>();

                services.AddTransient<FileView>();
                services.AddTransient<EdfPlotView>();
                services.AddTransient<SignalListView>();

                services.AddSingleton<PlotView>();
                services.AddSingleton<TimelineView>();
            });
        }
    }
}
