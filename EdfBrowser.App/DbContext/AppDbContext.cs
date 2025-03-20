using EdfBrowser.Model;
using Microsoft.EntityFrameworkCore;

namespace EdfBrowser.App
{
    internal class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        internal DbSet<FileItem> FileItems { get; set; }
        internal DbSet<ActionItem> ActionItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ActionItemConfigure());
        }
    }
}
