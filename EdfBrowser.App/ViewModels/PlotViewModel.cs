using EdfBrowser.Models;
using EdfBrowser.Services;
using Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class PlotViewModel
    {
        internal event EventHandler<IEnumerable<EdfSample>> SetSample;
        internal event EventHandler<EdfInfo> ConfigurePlot;

        private readonly IEdfParserService _edfParserService;
        private EdfSample[] _edfSamples;

        internal PlotViewModel(IEdfParserService edfParserService)
        {
            _edfParserService = edfParserService;
            OpenEdfFileCommnad = new AsyncRelayCommand(OpenEdf);
            ReadSamplesCommnad = new AsyncRelayCommand(ReadSamples);
        }


        internal ICommand OpenEdfFileCommnad { get; }
        internal ICommand ReadSamplesCommnad { get; }

        #region commands

        private async Task OpenEdf(object parameter)
        {
            string edfFilePath = parameter as string;

            if (edfFilePath == null)
                throw new ArgumentNullException($"{nameof(edfFilePath)}", "The parameter does not support the conversion of the EDF file path!");

            if (!File.Exists(edfFilePath))
                throw new FileNotFoundException($"Don`t find {edfFilePath}!", $"{nameof(edfFilePath)}");

            _edfParserService.Initial(edfFilePath);

            EdfInfo EdfInfo = await _edfParserService.ReadEdfInfo();

            _edfSamples = new EdfSample[EdfInfo.Signals];
            for (int i = 0; i < _edfSamples.Length; i++)
            {
                int channel = EdfInfo.SignalInfos[i].Channel;
                int sample = EdfInfo.SignalInfos[i].Samples;
                _edfSamples[i] = new EdfSample(channel, new double[sample]);
            }


            ConfigurePlot?.Invoke(this, EdfInfo);
        }

        private async Task ReadSamples(object parameter)
        {
            foreach (var sample in _edfSamples)
            {
                int count = await _edfParserService
                    .ReadPhysicalSamples(sample.Signal, sample.Buf);
            }

            SetSample?.Invoke(this, _edfSamples);
        }

        #endregion
    }
}
