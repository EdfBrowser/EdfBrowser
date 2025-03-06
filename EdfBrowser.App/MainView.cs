using EdfBrowser.Services;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    public partial class MainView : Form
    {
        private readonly EdfStore _edfStore;

        private readonly IMenuService _menuService;
        private readonly IEdfParserService _edfParserService;

        private readonly MenuViewModel _menuViewModel;
        private readonly EdfPlotViewModel _edfPlotViewModel;

        private readonly MenuView _menuView;
        private readonly EdfPlotView _edfPlotView;

        public MainView()
        {
            InitializeComponent();

            _menuService = new MenuService();
            _edfParserService = new EdfParserService();

            _edfStore = new EdfStore(_edfParserService);

            _menuViewModel = MenuViewModel.LoadMenus(_menuService, _edfStore);
            _edfPlotViewModel = new EdfPlotViewModel(_edfStore);

            _edfPlotView = new EdfPlotView(_edfPlotViewModel);
            _edfPlotView.Dock = DockStyle.Fill;
            _edfPlotView.Show();

            _menuView = new MenuView(_menuViewModel);
            _menuView.Height = 30;
            _menuView.Dock = DockStyle.Top;
            _menuView.Show();


            Controls.Add(_edfPlotView);
            Controls.Add(_menuView);
        }
    }
}
