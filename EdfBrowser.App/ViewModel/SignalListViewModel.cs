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

        internal SignalListViewModel(EdfStore edfStore)
        {
            _edfStore = edfStore;
            _edfStore.EdfFilePathChanged += OnEdfFilePathChanged;

            AddSignalCommand = new RelayCommand(AddSignal);
            RemoveSignalCommand = new RelayCommand(RemoveSignal);
            CompletedCommand = new RelayCommand(Completed);

            SignalItems = new ObservableCollection<SignalItem>();
            SelectedSignalItems = new ObservableCollection<SignalItem>();
        }

        internal ObservableCollection<SignalItem> SignalItems { get; private set; }
        internal ObservableCollection<SignalItem> SelectedSignalItems { get; private set; }
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

            SignalItems.Clear();

            for (int i = 0; i < _edfStore.EdfInfo._signalCount; i++)
            {
                SignalInfo signal = _edfStore.EdfInfo._signals[i];
                SignalItems.Add(new SignalItem() { Label = new string(signal._label), SampleRate = signal._samples });
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

                    SelectedSignalItems.Add(item);
                }
            }
        }

        private void RemoveSignal(object parameter)
        {
            if (parameter is IEnumerable<SignalItem> selectedItems)
            {
                foreach (SignalItem item in selectedItems)
                {
                    SelectedSignalItems.Remove(item);
                }
            }
        }

        private void Completed(object parameter)
        {

        }
    }
}
