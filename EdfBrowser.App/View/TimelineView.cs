using EdfBrowser.CustomControl;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal partial class TimelineView : UserControl
    {
        private readonly TimelineViewModel _timelineViewModel;
        private readonly ModernTimelineControl _timelineControl;

        // TODO: dispose event
        public TimelineView(TimelineViewModel timelineViewModel)
        {
            InitializeComponent();

            _timelineViewModel = timelineViewModel;
            _timelineViewModel.PropertyChanged += OnPropertyChanged;

            _timelineControl = new ModernTimelineControl
            {
                Dock = DockStyle.Fill
            };
            _timelineControl.ValueChanged += ValueChanged;

            Controls.Add(_timelineControl);

            Initial(0, _timelineViewModel.MaxValue);
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_timelineViewModel.MaxValue))
            {
                Initial(0, _timelineViewModel.MaxValue);
            }
        }

        private void ValueChanged(object sender, System.EventArgs e)
            => _timelineViewModel.CurrentValue = (uint)_timelineControl.CurrentValue;

        private void Initial(double minValue, double maxValue)
        {
            _timelineControl.CurrentValue = 0d;
            _timelineControl.MinValue = minValue;
            _timelineControl.MaxValue = maxValue;
        }
    }
}
