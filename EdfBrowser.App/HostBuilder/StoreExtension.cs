using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EdfBrowser.App
{
    internal static class StoreExtension
    {
        internal static IHostBuilder AddStores(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                services.AddSingleton<EdfStore>();
                services.AddSingleton<NavigationStore>();
            });
        }
    }
}
