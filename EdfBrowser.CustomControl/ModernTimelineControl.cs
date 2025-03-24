using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EdfBrowser.CustomControl
{
    public partial class ModernTimelineControl : Control
    {
        // 颜色配置
        private readonly Color _trackColor = Color.FromArgb(225, 225, 225);
        private readonly Color _sliderIdleColor = Color.FromArgb(140, 140, 140);
        private readonly Color _sliderDraggingColor = Color.FromArgb(80, 80, 80);

        // 尺寸配置
        private const float SPACE = 5f;
        private const float SliderMinWidth = 60f;
        private const float SliderRadius = 10f;

        // 数值范围
        private double _minValue = 0d;
        private double _maxValue = 100d;
        private double _currentValue = 0d;

        // 交互状态
        private bool _isDragging = false;
        private Cursor _defaultCursor;

        // button
        private const float ButtonWidth = 30f;
        private readonly Color _buttonColor = Color.FromArgb(180, 180, 180);
        private readonly Color _buttonHoverColor = Color.FromArgb(120, 120, 120);
        private bool _leftButtonHovered;
        private bool _rightButtonHovered;
        private RectangleF _leftButtonRect;
        private RectangleF _rightButtonRect;

        // track width 
        private float _railWidth => Width - SliderMinWidth - 2 * ButtonWidth;

        public ModernTimelineControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);

            BackColor = Color.White;
            Width = 100;
            Height = 50;
        }

        public event EventHandler ValueChanged;

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
            return (float)((current / total) * _railWidth) + ButtonWidth;
        }

        private double ToValue(float position)
        {
            double ratio = (double)(position - ButtonWidth) / _railWidth;
            return _minValue + (_maxValue - _minValue) * ratio;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            UpdateButtonRects();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);


        }

        private void UpdateButtonRects()
        {
            _leftButtonRect = new RectangleF(0, 0, ButtonWidth, Height);
            _rightButtonRect = new RectangleF(Width - ButtonWidth, 0, ButtonWidth, Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 绘制左右按钮
            DrawButton(g, _leftButtonRect, _leftButtonHovered, ArrowDirection.Left);
            DrawButton(g, _rightButtonRect, _rightButtonHovered, ArrowDirection.Right);


            // 轨道
            RectangleF railRect = new RectangleF(ButtonWidth, 0, Width - 2 * ButtonWidth, Height);
            using (SolidBrush railBrush = new SolidBrush(_trackColor))
            {
                g.FillRectangle(railBrush, railRect);
            }

            // 滑块 
            RectangleF sliderRect = GetSliderRect();

            Color sliderColor = _isDragging ? _sliderDraggingColor : _sliderIdleColor;
            using (SolidBrush slidingBrush = new SolidBrush(sliderColor))
            using (GraphicsPath path = CreateRoundedRect(sliderRect, SliderRadius))
            {
                g.FillPath(slidingBrush, path);
            }
        }

        private void DrawButton(Graphics g, RectangleF rect, bool hovered, ArrowDirection direction)
        {
            using (var path = CreateArrowPath(rect, direction))
            using (var brush = new SolidBrush(hovered ? _buttonHoverColor : _buttonColor))
            {
                g.FillPath(brush, path);
            }
        }

        private GraphicsPath CreateArrowPath(RectangleF rect, ArrowDirection direction)
        {
            var path = new GraphicsPath();
            float arrowSize = rect.Height * 0.3f;

            switch (direction)
            {
                case ArrowDirection.Left:
                    path.AddLines(new[] {
                        new PointF(rect.Right - arrowSize, rect.Top),
                        new PointF(rect.Left + arrowSize, rect.Top + rect.Height/2),
                        new PointF(rect.Right - arrowSize, rect.Bottom)
                    });
                    break;
                case ArrowDirection.Right:
                    path.AddLines(new[] {
                        new PointF(rect.Left + arrowSize, rect.Top),
                        new PointF(rect.Right - arrowSize, rect.Top + rect.Height/2),
                        new PointF(rect.Left + arrowSize, rect.Bottom)
                    });
                    break;
            }
            return path;
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

        private void GetMousePosition(MouseEventArgs e)
        {
            float newX = e.X;
            newX = NumericConversion.Clamp(newX, ButtonWidth, _railWidth + ButtonWidth);
            _currentValue = ToValue(newX);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // 处理拖动
            if (_isDragging)
            {
                GetMousePosition(e);
                Invalidate();
                return;
            }

            // 更新按钮悬停状态
            _leftButtonHovered = _leftButtonRect.Contains(e.Location);
            _rightButtonHovered = _rightButtonRect.Contains(e.Location);

            // 更新光标显示
            Cursor = _leftButtonHovered || _rightButtonHovered
                ? Cursors.Hand
                : _defaultCursor;

            // 请求重绘更新按钮状态
            if (_leftButtonHovered || _rightButtonHovered)
                Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (GetSliderRect().Contains(e.Location))
            {
                _isDragging = true;

                _defaultCursor = Cursor;
                Cursor = Cursors.Hand;
            }


            if (_leftButtonHovered)
            {
                CurrentValue = Math.Max(_minValue, CurrentValue - 5);
                ValueChanged?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (_rightButtonHovered)
            {
                CurrentValue = Math.Min(_maxValue, CurrentValue + 5);
                ValueChanged?.Invoke(this, EventArgs.Empty);
                return;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            _isDragging = false;
            Cursor = _defaultCursor;
            Invalidate();// 恢复原状

            ValueChanged?.Invoke(this, EventArgs.Empty);
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
