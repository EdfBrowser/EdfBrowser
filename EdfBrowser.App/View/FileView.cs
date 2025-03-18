using EdfBrowser.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal partial class FileView : UserControl
    {
        private class RecentFilesPanel : Panel
        {
            private const int ItemHeight = 60;
            private readonly FileView _owner;
            private int _hoverIndex = -1;

            internal RecentFilesPanel(FileView owner)
            {
                _owner = owner;
                AutoScroll = true;
                DoubleBuffered = true;
                SetStyle(ControlStyles.ResizeRedraw, true);
                BackColor = Color.White;
                Padding = new Padding(15);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                int yPos = Padding.Top;

                using (var titleFont = new Font("Segoe UI", 10, FontStyle.Bold))
                using (var subFont = new Font("Segoe UI", 9))
                using (var timeFont = new Font("Segoe UI", 8))
                {
                    for (int i = 0; i < _owner._recentFiles.Count; i++)
                    {
                        var item = _owner._recentFiles[i];

                        var rect = new Rectangle(Padding.Left, yPos,
                            ClientSize.Width - Padding.Horizontal, ItemHeight);

                        // 判断当前项是否是鼠标悬停项
                        if (i == _hoverIndex)
                        {
                            // 如果悬停，填充背景色为 AliceBlue
                            e.Graphics.FillRectangle(Brushes.AliceBlue, rect);
                        }

                        // 主标题
                        e.Graphics.DrawString(item.Title, titleFont, Brushes.Black,
                            rect.Left + 10, rect.Top + 10);

                        // 副标题
                        e.Graphics.DrawString(item.Subtitle, subFont, Brushes.Gray,
                            rect.Left + 10, rect.Top + 30);

                        // 时间戳
                        var timeSize = e.Graphics.MeasureString(item.Time.ToString(), timeFont);
                        e.Graphics.DrawString(item.Time.ToString("yyyy-MM-dd HH:mm"), timeFont, Brushes.DimGray,
                            rect.Right - timeSize.Width - 10, rect.Top + 10);

                        yPos += ItemHeight;
                    }
                }

                AutoScrollMinSize = new Size(0, yPos + Padding.Bottom);
            }

            protected override void OnMouseMove(MouseEventArgs e)
            {
                base.OnMouseMove(e);

                int index = (e.Y - Padding.Top) / ItemHeight;
                if (index != _hoverIndex)
                {
                    _hoverIndex = index;
                    Invalidate(); // 强制重新绘制面板
                }
            }

            protected override void OnMouseClick(MouseEventArgs e)
            {
                base.OnMouseClick(e);
                int index = (e.Y - Padding.Top) / ItemHeight;
                if (index >= 0 && index < _owner._recentFiles.Count)
                {
                    _owner._openFileCommand?.Execute(_owner._recentFiles[index]);
                }
            }
        }

        private class GetStartedPanel : Panel
        {
            private const int ButtonHeight = 70;
            private readonly FileView _owner;

            internal GetStartedPanel(FileView owner)
            {
                _owner = owner;
                AutoScroll = true;
                DoubleBuffered = true;
                SetStyle(ControlStyles.ResizeRedraw, true);
                BackColor = Color.FromArgb(245, 245, 245);
                Padding = new Padding(20);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                int yPos = Padding.Top;

                using (var titleFont = new Font("Segoe UI", 11, FontStyle.Bold))
                using (var subFont = new Font("Segoe UI", 9))
                {
                    foreach (var action in _owner._actionItems)
                    {
                        var rect = new Rectangle(Padding.Left, yPos,
                            ClientSize.Width - Padding.Horizontal, ButtonHeight);

                        // 绘制按钮背景
                        using (var brush = new SolidBrush(Color.White))
                        {
                            e.Graphics.FillRectangle(brush, rect);
                            e.Graphics.DrawRectangle(Pens.LightGray, rect);
                        }

                        // 主标题
                        e.Graphics.DrawString(action.Title, titleFont, Brushes.DodgerBlue,
                            rect.Left + 20, rect.Top + 15);

                        // 副标题
                        e.Graphics.DrawString(action.Description, subFont, Brushes.Gray,
                            rect.Left + 20, rect.Top + 40);

                        yPos += ButtonHeight + 10;
                    }
                }

                AutoScrollMinSize = new Size(0, yPos + Padding.Bottom);
            }

            protected override void OnMouseClick(MouseEventArgs e)
            {
                base.OnMouseClick(e);
                int index = (e.Y - Padding.Top) / (ButtonHeight + 10);
                if (index >= 0 && index < _owner._actionItems.Count)
                {
                    _owner._executeActionCommand?.Execute(null);
                }
            }
        }

        private readonly FileViewModel _fileViewModel;
        private readonly RecentFilesPanel _recentFilesPanel;
        private readonly GetStartedPanel _getStartedPanel;
        private List<FileItem> _recentFiles;
        private List<ActionItem> _actionItems;
        private ICommand _openFileCommand;
        private ICommand _executeActionCommand;

        internal FileView(FileViewModel fileViewModel)
        {
            InitializeComponent();

            _fileViewModel = fileViewModel;
            _fileViewModel.PropertyChanged += OnPropertyChanged;

            _recentFiles = _fileViewModel.RecentFiles;
            _actionItems = _fileViewModel.ActionItems;
            _openFileCommand = _fileViewModel.OpenFileCommand;
            _executeActionCommand = _fileViewModel.ExecuteActionCommand;

            _recentFilesPanel = new RecentFilesPanel(this)
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            _getStartedPanel = new GetStartedPanel(this)
            {
                Dock = DockStyle.Right,
                Width = 200,
                Padding = new Padding(10)
            };

            Controls.Add(_recentFilesPanel);
            Controls.Add(_getStartedPanel);
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_fileViewModel.RecentFiles))
            {
                _recentFiles = _fileViewModel.RecentFiles;
                _recentFilesPanel.Invalidate();
            }
            else if (e.PropertyName == nameof(_fileViewModel.ActionItems))
            {
                _actionItems = _fileViewModel.ActionItems;
            }
            else if (e.PropertyName == nameof(_fileViewModel.OpenFileCommand))
            {
                _openFileCommand = _fileViewModel.OpenFileCommand;
            }
            else if (e.PropertyName == nameof(_fileViewModel.ExecuteActionCommand))
            {
                _executeActionCommand = _fileViewModel.ExecuteActionCommand;
            }
        }
    }
}
