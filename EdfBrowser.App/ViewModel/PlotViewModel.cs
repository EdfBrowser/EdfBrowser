using EdfBrowser.EdfParser;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class PlotViewModel : BaseViewModel
    {
        private readonly EdfStore _edfStore;

        internal PlotViewModel(EdfStore edfStore)
        {
            _edfStore = edfStore;
            ReadSamplesCommnad = new AsyncRelayCommand(ReadSamples);
            ResetCommnad = new AsyncRelayCommand(Reset);
        }

        internal ICommand ReadSamplesCommnad { get; }
        internal ICommand ResetCommnad { get; }
        internal EdfInfo EdfInfo => _edfStore.EdfInfo;
        internal Sample[] EdfSamples { get; private set; }

        private Task Reset(object parameter)
        {
            EdfSamples = new Sample[(int)EdfInfo._signalCount];
            for (uint i = 0; i < EdfSamples.Length; i++)
            {
                uint sample = EdfInfo._signals[i]._samples;
                EdfSamples[i] = new Sample(sample, i);
            }

            OnPropertyChanged(nameof(EdfInfo));

            return Task.CompletedTask;
        }

        #region commands

        private async Task ReadSamples(object parameter)
        {
            if (EdfSamples == null) return;

            var startRecord = parameter as uint?;
            if (startRecord == null) return;

            foreach (var sample in EdfSamples)
            {
                sample.StartRecord = startRecord.Value;
                sample.ReadCount = 1;

                await _edfStore.ReadPhysicalSamples(sample);
            }

            OnPropertyChanged(nameof(EdfSamples));
        }

        #endregion
    }
}
