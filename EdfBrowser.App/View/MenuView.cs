using System;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal partial class MenuView : UserControl
    {
        internal event EventHandler<string> FileSelected;
        private readonly MenuViewModel _menuViewModel;

        internal MenuView(MenuViewModel menuViewModel)
        {
            InitializeComponent();

            _menuViewModel = menuViewModel;

            BackColor = System.Drawing.Color.White;

            Load += MenuView_Load;
        }

        private void MenuView_Load(object sender, System.EventArgs e)
        {
            // Create Menu Controls
            MenuStrip menuStrip = new MenuStrip();
            menuStrip.Dock = DockStyle.Fill;

            foreach (var menu in _menuViewModel.Menus)
            {
                ToolStripMenuItem rootMenuItem = new ToolStripMenuItem(menu.Description);

                if (menu.MenuItems != null)
                {
                    foreach (var item in menu.MenuItems)
                    {
                        ToolStripMenuItem subMenuItem = new ToolStripMenuItem(item.Description);
                        subMenuItem.Click += OnSubMenuItemClick;
                        rootMenuItem.DropDownItems.Add(subMenuItem);
                    }
                }

                menuStrip.Items.Add(rootMenuItem);
            }

            // Add the Menu Controls to the UserControl
            Controls.Add(menuStrip);
        }

        private void OnSubMenuItemClick(object sender, EventArgs e)
        {
            var clickedItem = sender as ToolStripMenuItem;
            if (clickedItem == null) return;

            var strategy = StrategyFactory.GetStrategy(clickedItem.Text);
            if (strategy != null)
            {
                if (strategy is OpenStrategy openStrategy)
                {
                    openStrategy.FileSelected += OnFileSelected;
                }
            }

            strategy?.Execute(); // 如果命令存在则执行
        }

        private void OnFileSelected(object sender, string path)
            => FileSelected?.Invoke(this, path);

        private interface IStrategy
        {
            void Execute();
        }

        private class OpenStrategy : IStrategy
        {
            internal event EventHandler<string> FileSelected;

            public void Execute()
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "EDF files (*.edf)|*.edf";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileSelected?.Invoke(this, openFileDialog.FileName);
                    }
                }
            }
        }

        private class CloseStrategy : IStrategy
        {
            public void Execute()
            {

            }
        }

        private static class StrategyFactory
        {
            internal static IStrategy GetStrategy(string description)
            {
                switch (description)
                {
                    case "Open":
                        return new OpenStrategy();
                    case "Close":
                        return new CloseStrategy();
                    // 可以添加更多的选项
                    default:
                        return null; // 默认情况
                }
            }
        }
    }
}
