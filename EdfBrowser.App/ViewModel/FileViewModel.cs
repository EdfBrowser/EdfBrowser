using EdfBrowser.Model;
using MVVMEssential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class FileViewModel : BaseViewModel
    {
        private readonly EdfStore _edfStore;
        private readonly NavigationService<SignalListViewModel> _navigationService;
        private readonly GenericDataService<FileItem> _fileItemService;
        private readonly GenericDataService<ActionItem> _actionItemService;
        private IEnumerable<FileItem> _recentFiles;
        private IEnumerable<ActionItem> _actionItems;

        public FileViewModel(EdfStore edfStore,
            NavigationService<SignalListViewModel> navigationService,
            GenericDataService<FileItem> fileItemService,
            GenericDataService<ActionItem> actionItemService)
        {
            _edfStore = edfStore;
            _navigationService = navigationService;
            _fileItemService = fileItemService;
            _actionItemService = actionItemService;


            OpenFileCommand = new AsyncRelayCommand(OpenFile);
            ExecuteActionCommand = new AsyncRelayCommand(ExecuteAction);
            LoadFileItemCommand = new AsyncRelayCommand(LoadFileItems);
            LoadActionItemCommand = new AsyncRelayCommand(LoadActionItems);


            LoadFileItemCommand.Execute(null);
            LoadActionItemCommand.Execute(null);
        }


        internal IEnumerable<FileItem> RecentFiles
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

        internal IEnumerable<ActionItem> ActionItems
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

        internal ICommand OpenFileCommand { get; }
        internal ICommand ExecuteActionCommand { get; }
        internal ICommand LoadFileItemCommand { get; }
        internal ICommand LoadActionItemCommand { get; }

        #region Command 

        private async Task LoadFileItems(object parameter)
        {
            RecentFiles = await _fileItemService.GetAll();
        }

        private async Task LoadActionItems(object arg)
        {
            ActionItems = await _actionItemService.GetAll();
        }

        private async Task OpenFile(object parameter)
        {
            if (parameter is FileItem fileItem)
            {
                fileItem.Time = DateTime.Now;

                await _fileItemService.Update(fileItem);

                _edfStore.SetFilePath(fileItem.Path);
                _navigationService.Navigate();
            }
        }

        private async Task ExecuteAction(object parameter)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "EDF files (*.edf)|*.edf";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = openFileDialog.FileName;
                    string subTitle = System.IO.Path.GetDirectoryName(path);
                    string title = System.IO.Path.GetFileName(path);
                    FileItem fileItem = new FileItem(title, subTitle, DateTime.Now);

                    await _fileItemService.Create(fileItem);

                    _edfStore.SetFilePath(openFileDialog.FileName);
                    _navigationService.Navigate();
                }
            }
        }

        #endregion
    }
}
