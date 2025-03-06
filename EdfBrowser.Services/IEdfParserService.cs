using EdfBrowser.EdfParser;
using System.Threading.Tasks;

namespace EdfBrowser.Services
{
    public interface IEdfParserService
    {
        void SetFilePath(string edfFilePath);

        Task<EdfInfo> ReadEdfInfo();
        Task<int> ReadPhysicalSamples(Sample sample);
    }
}
