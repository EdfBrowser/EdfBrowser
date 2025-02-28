using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EdfBrowser.CustomControl
{
    public partial class ModernTimelineControl : Control
    {
        // 颜色配置 (使用Edge滚动条配色)
        private readonly Color _trackColor = Color.FromArgb(225, 225, 225);
        private readonly Color _sliderIdleColor = Color.FromArgb(140, 140, 140);
        private readonly Color _sliderHoverColor = Color.FromArgb(100, 100, 100);
        private readonly Color _sliderDraggingColor = Color.FromArgb(80, 80, 80);

        // 尺寸配置
        private const float SPACE = 5f;
        private const float SliderMinWidth = 60f;
        private const float SliderRadius = 10f;

        // 数值范围
        private double _minValue = 0d;
        private double _maxValue = 100d;
        private double _currentValue = 5d;

        // 交互状态
        private bool _isDragging = false;
        private bool _isHovered = false;

        public event EventHandler ValueChanged;

        public ModernTimelineControl()
        {
            InitializeComponent();
            DoubleBuffered = true;

            BackColor = Color.White;
            Width = 100;
            Height = 50;

            Cursor = Cursors.Hand;
        }

        public double MinValue
        {
            get => _minValue;
            set
            {
                _minValue = value;
                Refresh();
            }
        }
        
        public double MaxValue
        {
            get => _maxValue;
            set
            {
                _maxValue = value;
                Refresh();
            }
        }

        public double CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                Refresh();
            }
        }


        private float ToPosition(double value)
        {
            double total = (_maxValue - _minValue);
            double current = (value - _minValue);
            double trackWidth = Width - SliderMinWidth;
            return (float)((current / total) * trackWidth);
        }

        private double ToValue(float position)
        {
            double trackWidth = Width - SliderMinWidth;
            double ratio = (double)position / trackWidth;
            return _minValue + (_maxValue - _minValue) * ratio;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 轨道
            using (SolidBrush railBrush = new SolidBrush(_trackColor))
            {
                Rectangle railRect = new Rectangle(0, 0, Width, Height);
                g.FillRectangle(railBrush, railRect);
            }

            // 滑块 
            RectangleF sliderRect = GetSliderRect();

            Color sliderColor = _isDragging ? _sliderDraggingColor :
                _isHovered ? _sliderHoverColor : _sliderIdleColor;
            using (SolidBrush slidingBrush = new SolidBrush(sliderColor))
            using (GraphicsPath path = CreateRoundedRect(sliderRect, SliderRadius))
            {
                g.FillPath(slidingBrush, path);
            }
        }

        private GraphicsPath CreateRoundedRect(RectangleF rect, float radius)
        {
            var path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            return path;
        }

        private RectangleF GetSliderRect()
        {
            float sliderX = ToPosition(_currentValue);
            return new RectangleF(sliderX, SPACE, SliderMinWidth, Height - 2 * SPACE);
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // 更新悬停状态
            bool wasHovered = _isHovered;
            _isHovered = GetSliderRect().Contains(e.Location);
            if (_isHovered != wasHovered) Refresh();

            // 处理拖动
            if (_isDragging)
            {
                float newX = e.X - SliderMinWidth / 2;
                newX = NumericConversion.Clamp(newX, 0, Width - SliderMinWidth);
                _currentValue = ToValue(newX);
                ValueChanged?.Invoke(this, EventArgs.Empty);
                Refresh();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (GetSliderRect().Contains(e.Location))
            {
                _isDragging = true;
                //Capture = true;

                Refresh();
            }
            else if (e.Y >= SPACE && e.Y < Height - 2 * SPACE) // 点击轨道跳转
            {
                _currentValue = ToValue(e.X - SliderMinWidth / 2);
                ValueChanged?.Invoke(this, EventArgs.Empty);
                Refresh();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            _isDragging = false;
            Refresh();
        }
    }

    internal static class NumericConversion
    {
        internal static T Clamp<T>(this T value, T min, T max) where T : IComparable
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;

            return value;
        }
    }
}
