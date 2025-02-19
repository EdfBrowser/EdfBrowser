using EdfBrowser.Models;
using Parser;
using Plot.Skia;
using Plot.WinForm;
using System.Collections.Generic;
using System.Linq;
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

            Load += PlotView_Load;
        }

        private void PlotView_Load(object sender, System.EventArgs e)
        {
            _figureForm.Dock = DockStyle.Fill;
            _figureForm.Show();
            Controls.Add(_figureForm);
        }

        internal void ConfigurePlot(object sender, EdfInfo edfInfo)
        {
            int ns = edfInfo.Signals;

            Figure figure = _figureForm.Figure;
            AxisManager axisManager = figure.AxisManager;
            SeriesManager seriesManager = figure.SeriesManager;


            IXAxis x = axisManager.DefaultBottom;
            x.ScrollMode = AxisScrollMode.Stepping;

            axisManager.Remove(Edge.Left);

            for (int i = 0; i < 16; i++)
            {
                IYAxis y = axisManager.AddNumericLeftAxis();

                double samples = 1.0 / edfInfo.SignalInfos[0].Samples;
                seriesManager.AddSignalSeries(x, y, new List<double>(), samples);
            }

            _figureForm.Refresh();
        }

        internal void SetSamples(object sender, IEnumerable<EdfSample> edfSamples)
        {
            Figure figure = _figureForm.Figure;
            SeriesManager seriesManager = figure.SeriesManager;

            for (int i = 0; i < seriesManager.Series.Count; i++)
            {
                EdfSample edfSample = edfSamples.ElementAt(i);
                var sig = (SignalSeries)seriesManager.Series[edfSample.Signal];
                var source = (SignalSouceDouble)sig.SignalSource;
                source.AddRange(edfSample.Buf);
                source.XOffset = sig.X.Min;
                sig.X.ScrollPosition = source.Length * source.SampleInterval;
            }


            figure.RenderManager.Fit();
            _figureForm.Refresh();
        }
    }
}
