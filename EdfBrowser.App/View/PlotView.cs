using EdfBrowser.EdfParser;
using Plot.Skia;
using Plot.WinForm;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal partial class PlotView : UserControl
    {
        private readonly PlotViewModel _plotViewModel;
        private readonly FigureForm _figureForm;

        internal PlotView(PlotViewModel plotViewModel)
        {
            InitializeComponent();

            _plotViewModel = plotViewModel;
            _plotViewModel.PropertyChanged += OnPropertyChanged;

            _figureForm = new FigureForm();
            _figureForm.Dock = DockStyle.Fill;
            _figureForm.Show();

            Controls.Add(_figureForm);
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Reset;
            if (e.PropertyName == nameof(PlotViewModel.EdfInfo))
            {
                Reset();
            }
            else if (e.PropertyName == nameof(PlotViewModel.EdfSamples))
            {
                SetSamples();
            }
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

        private void SetSamples()
        {
            Figure figure = _figureForm.Figure;
            SeriesManager seriesManager = figure.SeriesManager;

            for (int i = 0; i < seriesManager.Series.Count; i++)
            {
                Sample sample = _plotViewModel.EdfSamples[i];
                var sig = (SignalSeries)seriesManager.Series[(int)sample.Index];
                var source = (SignalSourceDouble)sig.SignalSource;
                source.AddRange(sample.Buffer);
                sig.X.ScrollPosition = source.Length * source.SampleInterval;
            }

            figure.RenderManager.Fit();
            _figureForm.Refresh();
        }
    }
}
