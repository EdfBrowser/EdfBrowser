using EdfBrowser.Model;
using MVVMEssential;
using Plot.Skia;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class PlotViewModel : BaseViewModel
    {
        private readonly EdfStore _edfStore;

        private Figure _figurePlot;
        private bool _isLoading;

        private string[] _mappingSignal;

        public PlotViewModel(EdfStore edfStore)
        {
            _edfStore = edfStore;
            _isLoading = false;

            ReadSamplesCommnad = new AsyncRelayCommand(ReadPhysicalSamples);
            ResetCommnad = new RelayCommand(ResetPlot);

            ResetCommnad.Execute(null);
        }

        internal ICommand ReadSamplesCommnad { get; }
        internal ICommand ResetCommnad { get; }

        internal Figure FigurePlot
        {
            get
            {
                if (_figurePlot == null)
                    _figurePlot = new Figure();

                return _figurePlot;
            }
            set
            {
                if (value != _figurePlot)
                {
                    _figurePlot = value;
                    OnPropertyChanged();
                }
            }
        }

        internal bool NeedsRefresh
        {
            set
            {
                if (value)
                    OnPropertyChanged();
            }
        }

        internal bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (value != _isLoading)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        #region commands

        private async Task ReadPhysicalSamples(object parameter)
        {
            IsLoading = true;

            RecordRange range = parameter as RecordRange;
            await _edfStore.ReadPhysicalSamples(range);

            UpdatePlot(range.Forward);

            IsLoading = false;
        }

        private void ResetPlot(object parameter)
        {
            AxisManager axisManager = FigurePlot.AxisManager;
            SeriesManager seriesManager = FigurePlot.SeriesManager;

            // reset
            axisManager.Remove(Edge.Bottom);
            IXAxis x = axisManager.AddNumericBottomAxis();
            x.ScrollMode = AxisScrollMode.Scrolling;

            axisManager.Remove(Edge.Left);

            _mappingSignal = new string[_edfStore.SelectedSignalItems.Count];
            int i = 0;

            foreach (SignalItem item in _edfStore.SelectedSignalItems)
            {
                IYAxis y = axisManager.AddNumericLeftAxis();
                y.Label.Text = item.Label;
                y.TickLabelStyle.Renderable = false;
                y.MajorTickStyle.Renderable = false;
                y.MinorTickStyle.Renderable = false;


                double samples = 1.0 / item.SampleRate;
                seriesManager.AddSignalSeries(x, y, samples);

                _mappingSignal[i++] = item.Label;
            }
        }

        private void UpdatePlot(bool forward)
        {
            SeriesManager seriesManager = FigurePlot.SeriesManager;
            AxisManager axisManager = FigurePlot.AxisManager;

            int i = 0;
            foreach (SignalSeries sig in seriesManager.Series)
            {
                string label = _mappingSignal[i++];
                DataRecord dataRecord = _edfStore.DataRecords[label];
                var source = (SignalSourceDouble)sig.SignalSource;

                if (forward)
                {
                    source.AddRange(dataRecord.Buffer);
                    //sig.X.ScrollPosition = source.Length * source.SampleInterval;
                }
                else
                {
                    source.PrependRange(dataRecord.Buffer);
                }

                axisManager.SetLimits(source.GetXLimit().ToRange, sig.X);

                dataRecord.Clear();
            }

            NeedsRefresh = true;
        }

        #endregion
    }
}
