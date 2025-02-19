using Parser;
using System.Threading.Tasks;

namespace EdfBrowser.Services
{
    public interface IEdfParserService
    {
        void Initial(string edfFilePath);

        Task<EdfInfo> ReadEdfInfo();
        Task<int> ReadPhysicalSamples(int signal, double[] buf);
    }
}
