using EdfBrowser.Model;
using EdfBrowser.Services;
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

        private MenuViewModel(IMenuService menuService, EdfStore edfStore)
        {
            _menuService = menuService;
            _edfStore = edfStore;
            LoadMenuCommand = new RelayCommand(LoadMenu);
            OpenFileCommand = new RelayCommand(OpenFile);
        }

        internal static MenuViewModel LoadMenus(IMenuService menuService, EdfStore edfStore)
        {
            MenuViewModel menuViewModel = new MenuViewModel(menuService, edfStore);
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
                    _edfStore.SetFilePath(openFileDialog.FileName);
                }
            }
        }
        #endregion
    }
}
