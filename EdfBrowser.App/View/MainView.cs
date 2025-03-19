using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EdfBrowser.App
{
    internal partial class MainView : Form
    {
        private readonly MainViewModel _mainViewModel;
        private readonly IServiceProvider _provider;
        private readonly Panel _contentPanel;

        private readonly Dictionary<Type, Type> _viewModelToViewMapping;

        public MainView(MainViewModel mainViewModel, IServiceProvider provider)
        {
            InitializeComponent();

            _mainViewModel = mainViewModel;
            _mainViewModel.PropertyChanged += OnPropertyChanged;

            _provider = provider;

            _viewModelToViewMapping = new Dictionary<Type, Type>
            {
                { typeof(FileViewModel), typeof(FileView) },
                { typeof(SignalListViewModel), typeof(SignalListView) },
                { typeof(EdfPlotViewModel), typeof(EdfPlotView) }
            };

            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            Controls.Add(_contentPanel);

            SwitchControl();
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_mainViewModel.CurrentViewModel))
                SwitchControl();
        }

        private void SwitchControl()
        {
            _contentPanel.Controls.Clear();

            Type currentViewModelType = _mainViewModel.CurrentViewModel?.GetType();

            if (currentViewModelType != null && _viewModelToViewMapping.ContainsKey(currentViewModelType))
            {
                Type viewType = _viewModelToViewMapping[currentViewModelType];
                UserControl view = _provider.GetRequiredService(viewType) as UserControl;

                if (view != null)
                {
                    _contentPanel.Controls.Add(view);
                }
            }
        }
    }
}
