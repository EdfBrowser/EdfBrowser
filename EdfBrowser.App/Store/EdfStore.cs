using EdfBrowser.EdfParser;
using EdfBrowser.Model;
using EdfBrowser.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            SignalItems = new ObservableCollection<SignalItem>();
            SelectedSignalItems = new ObservableCollection<SignalItem>();
        }


        internal event EventHandler EdfFilePathChanged;
        internal EdfInfo EdfInfo { get; private set; }
        internal Dictionary<string, DataRecord> DataRecords { get; private set; }

        internal ObservableCollection<SignalItem> SignalItems { get; private set; }
        internal ObservableCollection<SignalItem> SelectedSignalItems { get; private set; }

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
            DataRecords = new Dictionary<string, DataRecord>();
            for (uint i = 0; i < EdfInfo._signalCount; i++)
            {
                uint sampleRate = EdfInfo._signals[i]._samples;
                string label = new string(EdfInfo._signals[i]._label);
                DataRecords[label] = new DataRecord(sampleRate, i);
            }
        }

        internal async Task ReadPhysicalSamples(RecordRange range)
        {
            if (DataRecords == null)
                throw new ArgumentNullException(nameof(DataRecords), "Firstly call ReadInfo method.");

            foreach (SignalItem item in SelectedSignalItems)
            {
                await ReadPhysicalSamples(item.Label, range);
            }
        }

        internal async Task ReadPhysicalSamples(string label, RecordRange range)
        {
            if (DataRecords == null)
                throw new ArgumentNullException(nameof(DataRecords), "Firstly call ReadInfo method.");

            DataRecord dataRecord = DataRecords[label];
            if (range.Forward)
            {
                dataRecord.StartRecord = range.Start;
                dataRecord.ReadCount = range.End - range.Start;
            }
            else
            {
                dataRecord.StartRecord = range.End;
                dataRecord.ReadCount = range.Start - range.End;
            }

            await _edfParserService.ReadPhysicalSamples(dataRecord);
        }


        internal void Clear()
        {
            SignalItems.Clear();
            SelectedSignalItems.Clear();
        }

        internal void AddSignal(SignalItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            SignalItems.Add(item);
        }

        internal void AddSelectedSignal(SignalItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            SelectedSignalItems.Add(item);
        }

        internal void RemoveSelectedSignal(SignalItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            SelectedSignalItems.Remove(item);
        }

    }
}
