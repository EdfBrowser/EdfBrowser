using EdfBrowser.EdfParser;
using System.Threading.Tasks;

namespace EdfBrowser.Services
{
    public interface IEdfParserService
    {
        void Initial(string edfFilePath);

        Task<EdfInfo> ReadEdfInfo();
        Task<int> ReadPhysicalSamples(Sample sample);
    }
}
