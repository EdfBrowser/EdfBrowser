using EdfBrowser.EdfParser;
using EdfBrowser.Model;
using System.Threading.Tasks;

namespace EdfBrowser.Services
{
    public class EdfParserService : IEdfParserService
    {
        private EDFReader _reader;

        public void CreateInternalHandle(string edfFilePath)
        {
            _reader = new EDFReader(edfFilePath);
        }

        public HeaderInfo ReadHeaderInfo()
        {
            return _reader.ReadHeader();
        }

        public SignalInfo[] ReadSignalInfo(uint signalCount)
        {
            return _reader.ReadSignalInfo(signalCount);
        }

        public async Task ReadPhysicalSamples(DataRecord dataRecord)
        {
            // 用线程模拟
            await Task.Run(delegate
            {
                _reader.ReadPhysicalData(dataRecord);
            });
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }
}
