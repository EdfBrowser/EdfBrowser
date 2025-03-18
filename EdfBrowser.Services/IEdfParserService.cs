using EdfBrowser.EdfParser;
using EdfBrowser.Model;
using System.Threading.Tasks;

namespace EdfBrowser.Services
{
    public interface IEdfParserService
    {
        void SetFilePath(string edfFilePath);

        Task<EdfInfo> ReadEdfInfo();
        Task ReadPhysicalSamples(DataRecord dataRecord);
    }
}
