using System;

namespace EdfBrowser.App
{
    internal class TimelineViewModel : BaseViewModel
    {
        private double _maxValue;

        internal double MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (_maxValue != value)
                {
                    _maxValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _currentValue;
        internal double CurrentValue
        {
            get { return _currentValue; }
            set
            {
                if (value != _currentValue)
                {
                    _currentValue = value;
                    TimelineValueChanged?.Invoke(this, (uint)value);
                }
            }
        }

        internal event EventHandler<uint> TimelineValueChanged;
    }
}
