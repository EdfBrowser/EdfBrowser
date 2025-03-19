using Microsoft.EntityFrameworkCore;

namespace EdfBrowser.App
{
    internal class AppDbContextFactory : IAppDbContextFactory
    {
        private readonly string _connectionString;

        internal AppDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public AppDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connectionString).Options;

            return new AppDbContext(options);
        }
    }
}
