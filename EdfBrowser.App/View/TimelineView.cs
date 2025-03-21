using EdfBrowser.CustomControl;
using System;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal class TimelineView : BaseView
    {
        private readonly ModernTimelineControl _timelineControl;

        // TODO: dispose event
        public TimelineView()
        {
            _timelineControl = new ModernTimelineControl
            {
                Dock = DockStyle.Fill
            };
            _timelineControl.ValueChanged += ValueChanged;

            Controls.Add(_timelineControl);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!(DataContext is TimelineViewModel vm)) return;

            Initial(0, vm.MaxValue);
        }


        protected override void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!(DataContext is TimelineViewModel vm)) return;

            if (e.PropertyName == nameof(vm.MaxValue))
            {
                Initial(0, vm.MaxValue);
            }
        }

        private void ValueChanged(object sender, System.EventArgs e)
        {
            if (!(DataContext is TimelineViewModel vm)) return;

            vm.CurrentValue = (uint)_timelineControl.CurrentValue;
        }

        private void Initial(double minValue, double maxValue)
        {
            _timelineControl.CurrentValue = 0d;
            _timelineControl.MinValue = minValue;
            _timelineControl.MaxValue = maxValue;
        }
    }
}
