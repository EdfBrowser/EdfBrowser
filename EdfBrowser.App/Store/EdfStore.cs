using EdfBrowser.EdfParser;
using EdfBrowser.Model;
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


        internal event EventHandler EdfFilePathChanged;
        internal EdfInfo EdfInfo { get; private set; }
        internal DataRecord[] DataRecords { get; private set; }

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

            // 初始化
            DataRecords = new DataRecord[EdfInfo._signalCount];
            for (uint i = 0; i < DataRecords.Length; i++)
            {
                uint sampleRate = EdfInfo._signals[i]._samples;
                DataRecords[i] = new DataRecord(sampleRate, i);
            }
        }

        internal async Task ReadPhysicalSamples(RecordRange range)
        {
            if (DataRecords == null)
                throw new ArgumentNullException(nameof(DataRecords), "Firstly call ReadInfo method.");

            for (uint i = 0; i < DataRecords.Length; i++)
            {
                await ReadPhysicalSamples(i, range);
            }

        }

        internal async Task ReadPhysicalSamples(uint index, RecordRange range)
        {
            if (DataRecords == null)
                throw new ArgumentNullException(nameof(DataRecords), "Firstly call ReadInfo method.");

            DataRecord dataRecord = DataRecords[index];
            dataRecord.StartRecord = range.Start;
            dataRecord.ReadCount = range.End - range.Start;

            await _edfParserService.ReadPhysicalSamples(dataRecord);
        }

    }
}
