using EdfBrowser.EdfParser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class SignalListViewModel : BaseViewModel
    {
        private readonly EdfStore _edfStore;
        private readonly NavigationService<EdfPlotViewModel> _navigationService;

        internal SignalListViewModel(EdfStore edfStore,
            NavigationService<EdfPlotViewModel> navigationService)
        {
            _edfStore = edfStore;
            _edfStore.EdfFilePathChanged += OnEdfFilePathChanged;
            _navigationService = navigationService;

            AddSignalCommand = new RelayCommand(AddSignal);
            RemoveSignalCommand = new RelayCommand(RemoveSignal);
            CompletedCommand = new RelayCommand(Completed);
        }

        internal ObservableCollection<SignalItem> SignalItems => _edfStore.SignalItems;
        internal ObservableCollection<SignalItem> SelectedSignalItems => _edfStore.SelectedSignalItems;


        internal ICommand AddSignalCommand { get; }
        internal ICommand RemoveSignalCommand { get; }
        internal ICommand CompletedCommand { get; }

        protected override void Dispose(bool disposing)
        {
            _edfStore.EdfFilePathChanged -= OnEdfFilePathChanged;
        }

        private async void OnEdfFilePathChanged(object sender, EventArgs e)
        {
            await _edfStore.ReadInfo();

            _edfStore.Clear();

            for (int i = 0; i < _edfStore.EdfInfo._signalCount; i++)
            {
                SignalInfo signal = _edfStore.EdfInfo._signals[i];
                SignalItem signalItem = new SignalItem(signal);
                _edfStore.AddSignal(signalItem);
            }
        }


        private void AddSignal(object parameter)
        {
            if (parameter is IEnumerable<SignalItem> selectedItems)
            {
                foreach (SignalItem item in selectedItems)
                {
                    if (SelectedSignalItems.Contains(item))
                        continue;

                    _edfStore.AddSelectedSignal(item);
                }
            }
        }

        private void RemoveSignal(object parameter)
        {
            if (parameter is IEnumerable<SignalItem> selectedItems)
            {
                foreach (SignalItem item in selectedItems)
                {
                    _edfStore.RemoveSelectedSignal(item);
                }
            }
        }

        private void Completed(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
