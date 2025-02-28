using EdfBrowser.CustomControl;
using System;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal partial class TimelineView : UserControl
    {
        private readonly TimelineViewModel _timelineViewModel;
        private readonly ModernTimelineControl _timelineControl;

        internal event EventHandler<uint> TimelineValueChanged;

        internal TimelineView(TimelineViewModel timelineViewModel)
        {
            InitializeComponent();

            _timelineViewModel = timelineViewModel;

            _timelineControl = new ModernTimelineControl();
            _timelineControl.Dock = DockStyle.Fill;
            _timelineControl.ValueChanged += ValueChanged;

            Controls.Add(_timelineControl);
        }

        private void ValueChanged(object sender, System.EventArgs e)
            => TimelineValueChanged?.Invoke(this, (uint)_timelineControl.CurrentValue);

        internal void Initial(double minValue, double maxValue)
        {
            _timelineControl.CurrentValue = 0d;
            _timelineControl.MinValue = minValue;
            _timelineControl.MaxValue = maxValue;
        }
    }
}
