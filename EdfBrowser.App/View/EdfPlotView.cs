using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal partial class EdfPlotView : UserControl
    {
        private readonly EdfPlotViewModel _edfPlotViewModel;
        private readonly PlotView _plotView;
        private readonly TimelineView _timelineView;

        internal EdfPlotView(EdfPlotViewModel edfPlotViewModel)
        {
            InitializeComponent();

            _edfPlotViewModel = edfPlotViewModel;

            _plotView = new PlotView(_edfPlotViewModel.PlotViewModel);
            _plotView.Dock = DockStyle.Fill;
            _plotView.Show();

            _timelineView = new TimelineView(_edfPlotViewModel.TimelineViewModel);
            _timelineView.Height = 20;
            _timelineView.Dock = DockStyle.Bottom;
            _timelineView.Show();

            Controls.Add(_plotView);
            Controls.Add(_timelineView);
        }
    }
}
