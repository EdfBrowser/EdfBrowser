using EdfBrowser.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class FileViewModel : BaseViewModel
    {
        private readonly EdfStore _edfStore;
        private readonly NavigationService<SignalListViewModel> _navigationService;

        private List<FileItem> _recentFiles;
        private List<ActionItem> _actionItems;

        internal FileViewModel(EdfStore edfStore,
            NavigationService<SignalListViewModel> navigationService)
        {
            _edfStore = edfStore;
            _navigationService = navigationService;

            RecentFiles = new List<FileItem>
            {
                new FileItem("X.edf", "D:\\code\\", DateTime.Now),
            };

            ActionItems = new List<ActionItem>
            {
                new ActionItem("Open File", "Open local edf files")
            };

            OpenFileCommand = new RelayCommand(OpenFile);
            ExecuteActionCommand = new RelayCommand(ExecuteAction);
        }


        public List<FileItem> RecentFiles
        {
            get { return _recentFiles; }
            set
            {
                if (_recentFiles != value)
                {
                    _recentFiles = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<ActionItem> ActionItems
        {
            get { return _actionItems; }
            set
            {
                if (value != _actionItems)
                {
                    _actionItems = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand OpenFileCommand { get; }
        public ICommand ExecuteActionCommand { get; }

        #region Command 

        private void OpenFile(object parameter)
        {
            if (parameter is FileItem fileItem)
            {
                _navigationService.Navigate();
                _edfStore.SetFilePath(fileItem.Path);
            }
        }

        private void ExecuteAction(object parameter)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "EDF files (*.edf)|*.edf";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _navigationService.Navigate();
                    _edfStore.SetFilePath(openFileDialog.FileName);
                }
            }
        }

        #endregion
    }
}
