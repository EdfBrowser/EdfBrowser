namespace EdfBrowser.App
{
    internal interface IAppDbContextFactory
    {
        AppDbContext CreateDbContext();
    }
}
