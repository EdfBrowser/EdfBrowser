using System;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal class EdfPlotView : BaseView
    {
        private readonly PlotView _plotView;
        private readonly TimelineView _timelineView;
        private readonly FlowLayoutPanel _buttonPanel;
        private readonly Button _backwardButton;

        public EdfPlotView(PlotView plotView, TimelineView timelineView)
        {
            _plotView = plotView;
            _plotView.Dock = DockStyle.Fill;

            _timelineView = timelineView;
            _timelineView.Height = 20;
            _timelineView.Dock = DockStyle.Bottom;

            _buttonPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.LeftToRight,
            };

            _backwardButton = new Button
            {
                AutoSize = true,
                Text = "Backward"
            };
            _backwardButton.Click += OnBackward;

            _buttonPanel.Controls.Add(_backwardButton);

            Controls.Add(_plotView);
            Controls.Add(_timelineView);
            Controls.Add(_buttonPanel);
        }

        private void OnBackward(object sender, EventArgs e)
        {
            ((EdfPlotViewModel)DataContext)?.BackwardCommand.Execute(null);
        }

        internal void InitializeChildrens()
        {
            if (!(DataContext is EdfPlotViewModel vm)) return;

            _plotView.DataContext = vm.PlotViewModel;
            _timelineView.DataContext = vm.TimelineViewModel;
        }
    }
}
