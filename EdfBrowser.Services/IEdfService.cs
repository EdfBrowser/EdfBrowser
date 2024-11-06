using EdfBrowser.Models;

namespace EdfBrowser.Services
{
    public interface IEdfService
    {
        EdfInfo ParseEdf(string filePath);
    }
}
