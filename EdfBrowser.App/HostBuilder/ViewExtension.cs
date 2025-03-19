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

                services.AddSingleton(s =>
                    new FileView(s.GetRequiredService<FileViewModel>())
                    { Dock = System.Windows.Forms.DockStyle.Fill });
                services.AddSingleton(s =>
                    new EdfPlotView(s.GetRequiredService<EdfPlotViewModel>())
                    { Dock = System.Windows.Forms.DockStyle.Fill });
                services.AddSingleton(s =>
                    new SignalListView(s.GetRequiredService<SignalListViewModel>())
                    { Dock = System.Windows.Forms.DockStyle.Fill });
            });
        }
    }
}
