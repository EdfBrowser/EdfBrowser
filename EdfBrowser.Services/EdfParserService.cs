using EdfBrowser.EdfParser;
using System.Threading.Tasks;

namespace EdfBrowser.Services
{
    public class EdfParserService : IEdfParserService
    {
        private EDFReader _reader;

        public void Initial(string edfFilePath)
        {
            _reader?.Dispose();

            _reader = new EDFReader(edfFilePath);
        }

        public async Task<EdfInfo> ReadEdfInfo()
        {
            EdfInfo info = _reader.ReadHeader();

            return await Task.FromResult(info);
        }

        public async Task<int> ReadPhysicalSamples(Sample sample)
        {
            // 用线程模拟
            return await Task.Run(delegate
            {
                _reader.ReadPhysicalData(sample);
                return 0;
            });
        }
    }
}
