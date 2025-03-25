using EdfBrowser.EdfParser;
using EdfBrowser.Model;
using System;
using System.Threading.Tasks;

namespace EdfBrowser.Services
{
    public interface IEdfParserService : IDisposable
    {
        void CreateInternalHandle(string edfFilePath);

        HeaderInfo ReadHeaderInfo();
        SignalInfo[] ReadSignalInfo(uint signalCount);
        Task ReadPhysicalSamples(DataRecord dataRecord);
    }
}
