using EdfBrowser.EdfParser;
using EdfBrowser.Model;
using System;
using System.Threading.Tasks;

namespace EdfBrowser.Services
{
    public interface IEdfParserService : IDisposable
    {
        void CreateInternalHandle(string edfFilePath);

        Task<HeaderInfo> ReadEdfInfo();
        Task ReadPhysicalSamples(DataRecord dataRecord);
    }
}
