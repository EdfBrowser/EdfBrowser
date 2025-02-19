using Parser;
using System.IO;
using System.Threading.Tasks;

namespace EdfBrowser.Services
{
    public class EdfParserService : IEdfParserService
    {
        private readonly Reader _reader;

        public EdfParserService()
        {
            _reader = new Reader();
        }

        public void Initial(string edfFilePath)
        {
            _reader.Initial(File.OpenRead(edfFilePath));
        }

        public async Task<EdfInfo> ReadEdfInfo()
        {
            return await _reader.ReadEdfInfoAsync();
        }

        public async Task<int> ReadPhysicalSamples(int signal, double[] buf)
        {
            return await _reader.ReadDataAsync(signal, buf);
        }
    }
}
