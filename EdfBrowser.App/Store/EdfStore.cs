using EdfBrowser.EdfParser;
using EdfBrowser.Services;
using System;
using System.Threading.Tasks;

namespace EdfBrowser.App
{
    internal class EdfStore
    {
        private readonly IEdfParserService _edfParserService;
        private string _filePath = null;

        internal EdfStore(IEdfParserService edfParserService)
        {
            _edfParserService = edfParserService;
        }

        internal void SetFilePath(string edfFilePath)
        {
            if (string.IsNullOrEmpty(edfFilePath))
                throw new ArgumentNullException(nameof(edfFilePath));

            if (_filePath == edfFilePath)
                return;

            _filePath = edfFilePath;
            _edfParserService.SetFilePath(edfFilePath);
            EdfFilePathChanged?.Invoke(this, null);
        }

        internal async Task ReadInfo()
        {
            EdfInfo = await _edfParserService.ReadEdfInfo();
        }

        internal async Task<int> ReadPhysicalSamples(Sample sample)
        {
            return await _edfParserService.ReadPhysicalSamples(sample);
        }

        internal event EventHandler EdfFilePathChanged;
        internal EdfInfo EdfInfo { get; private set; }
    }
}
