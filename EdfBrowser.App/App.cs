
using EdfBrowser.App.Services;
using EdfBrowser.App.Store;
using EdfBrowser.App.View;
using EdfBrowser.App.ViewModels;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    public partial class App : Form
    {
        private readonly MenuService m_menuService;

        private readonly MenuStore m_menuStore;

        private readonly MenuViewModel m_menuViewModel;

        public App()
        {
            InitializeComponent();

            m_menuService = new MenuService();
            m_menuStore = new MenuStore(m_menuService);
            m_menuViewModel = new MenuViewModel(m_menuStore);

            Load += App_Load;
        }

        private void App_Load(object sender, System.EventArgs e)
        {
            MenuView menuView = new MenuView(m_menuViewModel);
            menuView.Dock = DockStyle.Top;
            menuView.Height = 30;


            Controls.Add(menuView);
        }
    }
}
