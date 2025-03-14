using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal partial class MainView : Form
    {
        private readonly MainViewModel _mainViewModel;

        private readonly MenuView _menuView;
        private readonly Panel _contentPanel;

        internal MainView(MainViewModel mainViewModel)
        {
            InitializeComponent();

            _mainViewModel = mainViewModel;
            _mainViewModel.PropertyChanged += OnPropertyChanged;

            _menuView = new MenuView(_mainViewModel.MenuViewModel);
            _menuView.Height = 30;
            _menuView.Dock = DockStyle.Top;
            _menuView.Show();


            _contentPanel = new Panel();
            _contentPanel.Dock = DockStyle.Fill;
            _contentPanel.Show();

            Controls.Add(_contentPanel);
            Controls.Add(_menuView);
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_mainViewModel.CurrentViewModel))
            {
                _contentPanel.Controls.Clear();

                if (_mainViewModel.CurrentViewModel is SignalListViewModel signalListViewModel)
                {
                    SignalListView signalListView = new SignalListView(signalListViewModel);
                    signalListView.Dock = DockStyle.Fill;
                    _contentPanel.Controls.Add(signalListView);
                }
                else if (_mainViewModel.CurrentViewModel is EdfPlotViewModel edfPlotViewModel)
                {
                    EdfPlotView edfPlotView = new EdfPlotView(edfPlotViewModel);
                    edfPlotView.Dock = DockStyle.Fill;
                    _contentPanel.Controls.Add(edfPlotView);
                }
            }
        }
    }
}
