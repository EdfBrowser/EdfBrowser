using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    // TODO：优化生命周期
    internal partial class EdfPlotView : UserControl
    {
        private readonly IServiceProvider _provider;
        private readonly PlotView _plotView;
        private readonly TimelineView _timelineView;

        public EdfPlotView(IServiceProvider provider)
        {
            InitializeComponent();

            _provider= provider;

            _plotView = _provider.GetRequiredService<PlotView>();
            _plotView.Dock = DockStyle.Fill;

            _timelineView = _provider.GetRequiredService<TimelineView>();
            _timelineView.Height = 20;
            _timelineView.Dock = DockStyle.Bottom;

            Controls.Add(_plotView);
            Controls.Add(_timelineView);
        }
    }
}
