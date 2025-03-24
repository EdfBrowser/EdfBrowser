using EdfBrowser.EdfParser;
using MVVMEssential;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class SignalListViewModel : BaseViewModel
    {
        private readonly EdfStore _edfStore;
        private readonly NavigationService<EdfPlotViewModel> _navigationEdfPlotService;
        private readonly NavigationService<FileViewModel> _navigationFileService;

        public SignalListViewModel(EdfStore edfStore,
            NavigationService<EdfPlotViewModel> navigationEdfPlotService,
            NavigationService<FileViewModel> navigationFileService)
        {
            _edfStore = edfStore;
            _navigationEdfPlotService = navigationEdfPlotService;
            _navigationFileService = navigationFileService;

            AddSignalCommand = new RelayCommand(AddSignal);
            RemoveSignalCommand = new RelayCommand(RemoveSignal);
            CompletedCommand = new RelayCommand(Completed);
            BackwardCommand = new RelayCommand(Backward);
        }

        internal ObservableCollection<SignalItem> SignalItems => _edfStore.SignalItems;
        internal ObservableCollection<SignalItem> SelectedSignalItems => _edfStore.SelectedSignalItems;

        internal ICommand AddSignalCommand { get; }
        internal ICommand RemoveSignalCommand { get; }
        internal ICommand CompletedCommand { get; }
        internal ICommand BackwardCommand { get; }

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
            _navigationEdfPlotService.Navigate();
        }

        private void Backward(object parameter)
        {
            _navigationFileService.Navigate();
        }
    }
}
