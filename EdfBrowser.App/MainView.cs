using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal partial class MainView : Form
    {
        private readonly MainViewModel _mainViewModel;

        private readonly Panel _contentPanel;

        internal MainView(MainViewModel mainViewModel)
        {
            InitializeComponent();

            _mainViewModel = mainViewModel;
            _mainViewModel.PropertyChanged += OnPropertyChanged;

            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            Controls.Add(_contentPanel);
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_mainViewModel.CurrentViewModel))
            {
                _contentPanel.Controls.Clear();

                if (_mainViewModel.CurrentViewModel is FileViewModel fileViewModel)
                {
                    FileView fileView = new FileView(fileViewModel)
                    {
                        Dock = DockStyle.Fill
                    };
                    _contentPanel.Controls.Add(fileView);
                }
                else if (_mainViewModel.CurrentViewModel is SignalListViewModel signalListViewModel)
                {
                    SignalListView signalListView = new SignalListView(signalListViewModel)
                    {
                        Dock = DockStyle.Fill
                    };
                    _contentPanel.Controls.Add(signalListView);
                }
                else if (_mainViewModel.CurrentViewModel is EdfPlotViewModel edfPlotViewModel)
                {
                    EdfPlotView edfPlotView = new EdfPlotView(edfPlotViewModel)
                    {
                        Dock = DockStyle.Fill
                    };
                    _contentPanel.Controls.Add(edfPlotView);
                }
            }
        }
    }
}
