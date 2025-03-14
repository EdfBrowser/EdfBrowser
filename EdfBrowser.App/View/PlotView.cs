using EdfBrowser.EdfParser;
using EdfBrowser.Model;
using Plot.Skia;
using Plot.WinForm;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal partial class PlotView : UserControl
    {
        private readonly PlotViewModel _plotViewModel;
        private readonly FigureForm _figureForm;
        private readonly Dictionary<string, Action> _actions;

        internal PlotView(PlotViewModel plotViewModel)
        {
            InitializeComponent();

            _actions = new Dictionary<string, Action>()
            {
                {nameof(PlotViewModel.EdfInfo), Reset },
                {nameof(PlotViewModel.DataRecords), UpdatedPlot },
            };

            _plotViewModel = plotViewModel;
            _plotViewModel.PropertyChanged += OnPropertyChanged;

            _figureForm = new FigureForm();
            _figureForm.Dock = DockStyle.Fill;
            _figureForm.Show();

            Controls.Add(_figureForm);
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _actions.TryGetValue(e.PropertyName, out Action action);
            action.Invoke();
        }

        private void Reset()
        {
            EdfInfo info = _plotViewModel.EdfInfo;
            uint ns = info._signalCount;

            Figure figure = _figureForm.Figure;
            AxisManager axisManager = figure.AxisManager;
            SeriesManager seriesManager = figure.SeriesManager;

            // reset
            axisManager.Remove(Edge.Bottom);
            IXAxis x = axisManager.AddNumericBottomAxis();
            x.ScrollMode = AxisScrollMode.Scrolling;

            axisManager.Remove(Edge.Left);

            for (int i = 0; i < 8; i++)
            {
                IYAxis y = axisManager.AddNumericLeftAxis();

                double samples = 1.0 / info._signals[i]._samples;
                seriesManager.AddSignalSeries(x, y, samples);
            }

            _figureForm.Refresh();
        }

        private void UpdatedPlot()
        {
            Figure figure = _figureForm.Figure;
            SeriesManager seriesManager = figure.SeriesManager;

            for (int i = 0; i < seriesManager.Series.Count; i++)
            {
                DataRecord dataRecord = _plotViewModel.DataRecords[i];
                var sig = (SignalSeries)seriesManager.Series[(int)dataRecord.Index];
                var source = (SignalSourceDouble)sig.SignalSource;
                source.AddRange(dataRecord.Buffer);
                sig.X.ScrollPosition = source.Length * source.SampleInterval;
            }

            figure.RenderManager.Fit();
            _figureForm.Refresh();
        }
    }
}
