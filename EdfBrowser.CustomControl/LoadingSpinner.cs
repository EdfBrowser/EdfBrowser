using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace EdfBrowser.CustomControl
{
    public partial class LoadingSpinner : Control
    {
        private float _angle;
        private readonly Timer _timer;

        public LoadingSpinner()
        {
            InitializeComponent();

            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);

            _timer = new Timer { Interval = 50 };
            _timer.Tick += (s, e) =>
            {
                _angle = (_angle + 30) % 360;
                Invalidate();
            };
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            _timer.Enabled = Visible;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var pen = new Pen(Color.DodgerBlue, 3))
            using (var brush = new SolidBrush(Color.FromArgb(100, Color.DodgerBlue)))
            {
                e.Graphics.TranslateTransform(Width / 2f, Height / 2f);
                e.Graphics.RotateTransform(_angle);

                e.Graphics.DrawArc(pen, -15, -15, 30, 30, 0, 270);
                e.Graphics.FillEllipse(brush, -5, -5, 10, 10);
            }
        }
    }
}
