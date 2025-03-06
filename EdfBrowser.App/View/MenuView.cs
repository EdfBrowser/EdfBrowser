using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal partial class MenuView : UserControl
    {
        private readonly MenuViewModel _menuViewModel;

        internal MenuView(MenuViewModel menuViewModel)
        {
            InitializeComponent();

            _menuViewModel = menuViewModel;

            BackColor = System.Drawing.Color.White;

            InitializedMenu();
        }

        private void InitializedMenu()
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
                        if (item.Description == "Open")
                            subMenuItem.Click += delegate { _menuViewModel.OpenFileCommand.Execute(null); };

                        rootMenuItem.DropDownItems.Add(subMenuItem);
                    }
                }

                menuStrip.Items.Add(rootMenuItem);
            }

            Controls.Add(menuStrip);
        }
    }
}
