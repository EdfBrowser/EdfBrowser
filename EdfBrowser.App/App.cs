using EdfBrowser.Services;
using System;
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
        private readonly TimelineViewModel _timelineViewModel;

        private readonly PlotView _plotView;
        private readonly MenuView _menuView;
        private readonly TimelineView _timelineView;

        public App()
        {
            InitializeComponent();


            _menuService = new MenuService();
            _edfParserService = new EdfParserService();

            _menuStore = new MenuStore(_menuService);

            _menuViewModel = MenuViewModel.LoadMenus(_menuStore);
            _plotViewModel = new PlotViewModel(_edfParserService);
            _timelineViewModel = new TimelineViewModel();

            _menuView = new MenuView(_menuViewModel);
            _menuView.Height = 30;
            _menuView.Dock = DockStyle.Top;
            _menuView.Show();
            _menuView.FileSelected += OnFileSelected;

            _plotView = new PlotView(_plotViewModel);
            _plotView.Dock = DockStyle.Fill;
            _plotView.Show();

            _timelineView = new TimelineView(_timelineViewModel);
            _timelineView.Height = 20;
            _timelineView.Dock = DockStyle.Bottom;
            _timelineView.Enabled = false;
            _timelineView.Show();
            _timelineView.TimelineValueChanged += TimelineValueChanged;

            Controls.Add(_plotView);
            Controls.Add(_menuView);
            Controls.Add(_timelineView);
        }

        private void TimelineValueChanged(object sender, uint currentValue)
        {
            System.Diagnostics.Debug.WriteLine(currentValue);
            _plotViewModel.ReadSamplesCommnad.Execute(currentValue);
        }

        private void OnFileSelected(object sender, string path)
        {
            _plotViewModel.OpenEdfFileCommnad.Execute(path);
            _timelineView.Enabled = true;

            //  «∑Ò”≈ªØ
            double total = _plotViewModel.EdfInfo._recordCount * _plotViewModel.EdfInfo._recordDuration;
            _timelineView.Initial( 0, total);
        }
    }
}
