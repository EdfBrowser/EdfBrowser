using EdfBrowser.Model;
using EdfBrowser.Services;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class MenuViewModel
    {
        private readonly Dictionary<string, List<string>> _mocks = new Dictionary<string, List<string>>()
        {
            {"File", new List<string> (){ "Open", "Close" } },
        };

        private readonly IMenuService _menuService;
        private readonly EdfStore _edfStore;
        private readonly NavigationService<SignalListViewModel> _navigationService;

        private MenuViewModel(IMenuService menuService, EdfStore edfStore, 
            NavigationService<SignalListViewModel> navigationService)
        {
            _menuService = menuService;
            _edfStore = edfStore;
            _navigationService = navigationService;

            LoadMenuCommand = new RelayCommand(LoadMenu);
            OpenFileCommand = new RelayCommand(OpenFile);
        }

        internal static MenuViewModel LoadMenus(IMenuService menuService, EdfStore edfStore, 
            NavigationService<SignalListViewModel> navigationService)
        {
            MenuViewModel menuViewModel = new MenuViewModel(menuService, edfStore, navigationService);
            menuViewModel.LoadMenuCommand.Execute(null);
            return menuViewModel;
        }

        internal ICommand LoadMenuCommand { get; }
        internal ICommand OpenFileCommand { get; }
        internal IEnumerable<MenuStructure> Menus { get; private set; }

        #region commands
        private void LoadMenu(object parameter)
        {
            Menus = _menuService.CreateMenuStructure(_mocks);
        }

        private void OpenFile(object parameter)
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
