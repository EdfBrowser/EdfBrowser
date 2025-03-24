using EdfBrowser.Model;
using EdfBrowser.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EdfBrowser.App
{
    internal static class ServiceExtension
    {
        internal static IHostBuilder AddServices(this IHostBuilder host)
        {
            return host.ConfigureServices(services =>
            {
                services.AddSingleton<IEdfParserService, EdfParserService>();
                services.AddSingleton<GenericDataService<FileItem>>();
                services.AddSingleton<GenericDataService<ActionItem>>();
            });
        }
    }
}
