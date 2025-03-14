using EdfBrowser.EdfParser;
using EdfBrowser.Model;
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
            ReadSamplesCommnad = new AsyncRelayCommand(ReadPhysicalSamples);
            ResetCommnad = new RelayCommand(ResetedPlot);
            UpdatedPlotCommnad = new RelayCommand(UpdatedPlot);
        }

        internal ICommand ReadSamplesCommnad { get; }
        internal ICommand ResetCommnad { get; }
        internal ICommand UpdatedPlotCommnad { get; }

        // 虚拟字段用触发propertyChanged
        internal EdfInfo EdfInfo => _edfStore.EdfInfo;
        internal DataRecord[] DataRecords => _edfStore.DataRecords;

        #region commands

        private async Task ReadPhysicalSamples(object parameter)
        {
            RecordRange range = parameter as RecordRange;
            await _edfStore.ReadPhysicalSamples(range);

            OnPropertyChanged(nameof(DataRecords));
        }

        private void ResetedPlot(object parameter)
        {
            OnPropertyChanged(nameof(EdfInfo));
        }

        private void UpdatedPlot(object parameter)
        {
            OnPropertyChanged(nameof(DataRecords));
        }

        #endregion
    }
}
