using EdfBrowser.EdfParser;
using System.Threading.Tasks;
using EdfBrowser.Model;

namespace EdfBrowser.Services
{
    public interface IEdfParserService
    {
        void SetFilePath(string edfFilePath);

        Task<EdfInfo> ReadEdfInfo();
        Task ReadPhysicalSamples(DataRecord dataRecord);
    }
}
