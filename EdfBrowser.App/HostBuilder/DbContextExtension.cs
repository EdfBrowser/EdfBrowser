using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EdfBrowser.App
{
    internal static class DbContextExtension
    {
        internal static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            return host.ConfigureServices((context, services) =>
            {
                string connectionString = context.Configuration.GetConnectionString("sqlite");

#if DEBUG
                services.AddSingleton<IAppDbContextFactory>(new InMemoryAppDbContext());
#else
                services.AddSingleton<IAppDbContextFactory>(new AppDbContextFactory(connectionString));
#endif
            });
        }
    }
}
