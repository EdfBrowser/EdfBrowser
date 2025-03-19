using EdfBrowser.Model;
using Microsoft.EntityFrameworkCore;

namespace EdfBrowser.App
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        internal DbSet<FileItem> FileItems { get; set; }
    }
}
