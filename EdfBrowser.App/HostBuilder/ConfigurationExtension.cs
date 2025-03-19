using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace EdfBrowser.App
{
    internal static class ConfigurationExtension
    {
        internal static IHostBuilder AddConfiguration(this IHostBuilder host)
        {
            return host.ConfigureAppConfiguration(c =>
            {
                c.AddJsonFile("appsettings.json");
            });
        }
    }
}
