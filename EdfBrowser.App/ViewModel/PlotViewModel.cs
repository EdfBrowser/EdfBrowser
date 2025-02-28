using EdfBrowser.EdfParser;
using EdfBrowser.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class PlotViewModel
    {
        // Event
        internal event EventHandler SetSample;
        internal event EventHandler ConfigurePlot;

        private readonly IEdfParserService _edfParserService;

        internal PlotViewModel(IEdfParserService edfParserService)
        {
            _edfParserService = edfParserService;
            OpenEdfFileCommnad = new AsyncRelayCommand(OpenEdf);
            ReadSamplesCommnad = new AsyncRelayCommand(ReadSamples);
        }


        internal ICommand OpenEdfFileCommnad { get; }
        internal ICommand ReadSamplesCommnad { get; }
        internal EdfInfo EdfInfo { get; private set; }
        internal Sample[] EdfSamples { get; private set; }

        #region commands

        private async Task OpenEdf(object parameter)
        {
            string edfFilePath = parameter as string;

            if (edfFilePath == null)
                throw new ArgumentNullException($"{nameof(edfFilePath)}", "The parameter does not support the conversion of the EDF file path!");

            if (!File.Exists(edfFilePath))
                throw new FileNotFoundException($"Don`t find {edfFilePath}!", $"{nameof(edfFilePath)}");

            _edfParserService.Initial(edfFilePath);

            // TODO: 是否为一个单独的行为
            EdfInfo = await _edfParserService.ReadEdfInfo();

            EdfSamples = new Sample[(int)EdfInfo._signalCount];
            for (uint i = 0; i < EdfSamples.Length; i++)
            {
                uint sample = EdfInfo._signals[i]._samples;
                EdfSamples[i] = new Sample(sample, i);
            }

            ConfigurePlot?.Invoke(this, null);
        }

        private async Task ReadSamples(object parameter)
        {
            var startRecord = parameter as uint?;
            if (startRecord == null) startRecord = 0;

            foreach (var sample in EdfSamples)
            {
                sample.StartRecord = startRecord.Value;
                sample.ReadCount = 1;

                int count = await _edfParserService
                    .ReadPhysicalSamples(sample);
            }

            SetSample?.Invoke(this, null);
        }

        #endregion
    }
}
