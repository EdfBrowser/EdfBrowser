using EdfBrowser.EdfParser;
using EdfBrowser.Model;
using System.Threading.Tasks;

namespace EdfBrowser.Services
{
    public class EdfParserService : IEdfParserService
    {
        private EDFReader _reader;

        public void SetFilePath(string edfFilePath)
        {
            _reader?.Dispose();

            _reader = new EDFReader(edfFilePath);
        }

        public async Task<EdfInfo> ReadEdfInfo()
        {
            EdfInfo info = _reader.ReadHeader();

            return await Task.FromResult(info);
        }

        public async Task ReadPhysicalSamples(DataRecord dataRecord)
        {
            // 用线程模拟
            await Task.Run(delegate
            {
                _reader.ReadPhysicalData(dataRecord);
            });
        }
    }
}
