
using EdfBrowser.App.Store;
using EdfBrowser.App.View;
using EdfBrowser.App.ViewModels;
using EdfBrowser.Services;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    public partial class App : Form
    {
        private readonly IMenuService m_menuService;
        private readonly IEdfService m_edfService;

        private readonly MenuStore m_menuStore;
        private readonly EdfStore m_edfStore;

        private readonly MenuViewModel m_menuViewModel;
        private readonly EdfDashBoardViewModel m_edfDashBoardViewModel;

        public App()
        {
            InitializeComponent();


            m_edfService = new EdfService();
            m_menuService = new MenuService();

            m_edfStore = new EdfStore(m_edfService);
            m_menuStore = new MenuStore(m_menuService);

            m_edfDashBoardViewModel = new EdfDashBoardViewModel(m_edfStore);
            m_menuViewModel = MenuViewModel.LoadMenus(m_menuStore, m_edfDashBoardViewModel);

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
