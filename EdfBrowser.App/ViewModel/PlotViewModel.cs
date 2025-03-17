using EdfBrowser.EdfParser;
using EdfBrowser.Model;
using Plot.Skia;
using Plot.WinForm;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class PlotViewModel : BaseViewModel
    {
        private readonly EdfStore _edfStore;

        private Figure _figurePlot;
        private bool _isLoading;

        internal PlotViewModel(EdfStore edfStore)
        {
            _edfStore = edfStore;
            _isLoading = false;

            ReadSamplesCommnad = new AsyncRelayCommand(ReadPhysicalSamples);
            ResetCommnad = new RelayCommand(ResetPlot);
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

            foreach (SignalItem item in _edfStore.SelectedSignalItems)
            {
                IYAxis y = axisManager.AddNumericLeftAxis();

                double samples = 1.0 / item.SampleRate;
                seriesManager.AddSignalSeries(x, y, samples);
            }
        }

        private void UpdatePlot(bool forward)
        {
            SeriesManager seriesManager = FigurePlot.SeriesManager;
            AxisManager axisManager = FigurePlot.AxisManager;

            for (int i = 0; i < seriesManager.Series.Count; i++)
            {
                DataRecord dataRecord = _edfStore.DataRecords[i];
                var sig = (SignalSeries)seriesManager.Series[(int)dataRecord.Index];
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

                axisManager.SetLimits(sig.GetXLimit().ToRange, sig.X);
            }

            NeedsRefresh = true;
        }

        #endregion
    }
}
