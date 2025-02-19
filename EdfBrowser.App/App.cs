using EdfBrowser.Services;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    public partial class App : Form
    {
        private readonly IMenuService _menuService;
        private readonly IEdfParserService _edfParserService;

        private readonly MenuStore _menuStore;

        private readonly MenuViewModel _menuViewModel;
        private readonly PlotViewModel _plotViewModel;

        private readonly PlotView _plotView;
        public App()
        {
            InitializeComponent();


            _menuService = new MenuService();
            _edfParserService = new EdfParserService();

            _menuStore = new MenuStore(_menuService);

            _menuViewModel = MenuViewModel.LoadMenus(_menuStore);
            _plotViewModel = new PlotViewModel(_edfParserService);

            _plotView = new PlotView(_plotViewModel);


            Load += App_Load;
        }

        private void App_Load(object sender, System.EventArgs e)
        {
            MenuView menuView = new MenuView(_menuViewModel);
            menuView.Height = 30;
            menuView.FileSelected += OnFileSelected;
            menuView.Dock = DockStyle.Top;
            menuView.Show();

            _plotView.Dock = DockStyle.Fill;
            _plotView.Show();

            Controls.Add(_plotView);
            Controls.Add(menuView);
        }

        private void OnFileSelected(object sender, string path)
        {
            _plotViewModel.OpenEdfFileCommnad.Execute(path);

            Timer timer = new Timer() { Enabled = true, Interval = 500 };
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            _plotViewModel.ReadSamplesCommnad.Execute(null);
        }
    }
}
