using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EdfBrowser.App
{
    internal class InMemoryAppDbContext : IAppDbContextFactory
    {
        private readonly SqliteConnection _connection;

        internal InMemoryAppDbContext()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();
        }

        public AppDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(_connection).Options;

            return new AppDbContext(options);
        }
    }
}
