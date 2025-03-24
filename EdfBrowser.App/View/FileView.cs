using EdfBrowser.Model;
using MVVMEssential;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class FileView : BaseView
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
                if (_owner._recentFiles == null)
                    return;

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
                if (_owner._recentFiles == null)
                    return;

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

        private readonly RecentFilesPanel _recentFilesPanel;
        private readonly GetStartedPanel _getStartedPanel;
        private IList<FileItem> _recentFiles;
        private IList<ActionItem> _actionItems;
        private ICommand _openFileCommand;
        private ICommand _executeActionCommand;

        public FileView()
        {
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!(DataContext is FileViewModel vm)) return;

            _recentFiles = vm.RecentFiles.ToList();
            _actionItems = vm.ActionItems.ToList();
            _openFileCommand = vm.OpenFileCommand;
            _executeActionCommand = vm.ExecuteActionCommand;
        }

        protected override void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!(DataContext is FileViewModel vm)) return;

            if (e.PropertyName == nameof(vm.RecentFiles))
            {
                _recentFiles = vm.RecentFiles.ToList();
                _recentFilesPanel.Invalidate();
            }
            else if (e.PropertyName == nameof(vm.ActionItems))
            {
                _actionItems = vm.ActionItems.ToList();
                _getStartedPanel.Invalidate();
            }
            else if (e.PropertyName == nameof(vm.OpenFileCommand))
            {
                _openFileCommand = vm.OpenFileCommand;
            }
            else if (e.PropertyName == nameof(vm.ExecuteActionCommand))
            {
                _executeActionCommand = vm.ExecuteActionCommand;
            }
        }
    }
}
