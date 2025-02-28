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
            _plotViewModel.SetSample += SetSamples;
            _plotViewModel.ConfigurePlot += ConfigurePlot;

            _figureForm = new FigureForm();
            _figureForm.Dock = DockStyle.Fill;
            _figureForm.Show();

            Controls.Add(_figureForm);
        }

        internal void ConfigurePlot(object sender, System.EventArgs e)
        {
            EdfInfo info = _plotViewModel.EdfInfo;
            uint ns = info._signalCount;

            Figure figure = _figureForm.Figure;
            AxisManager axisManager = figure.AxisManager;
            SeriesManager seriesManager = figure.SeriesManager;


            IXAxis x = axisManager.DefaultBottom;
            x.ScrollMode = AxisScrollMode.Scrolling;

            axisManager.Remove(Edge.Left);

            for (int i = 0; i < 16; i++)
            {
                IYAxis y = axisManager.AddNumericLeftAxis();

                double samples = 1.0 / info._signals[i]._samples;
                seriesManager.AddSignalSeries(x, y, samples);
            }

            _figureForm.Refresh();
        }

        internal void SetSamples(object sender, System.EventArgs e)
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
