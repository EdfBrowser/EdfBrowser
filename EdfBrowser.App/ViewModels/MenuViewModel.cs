using EdfBrowser.Models;
using System.Collections.Generic;
using System.Windows.Input;

namespace EdfBrowser.App
{
    internal class MenuViewModel
    {
        private readonly MenuStore _menuStore;

        private MenuViewModel(MenuStore menuStore)
        {
            _menuStore = menuStore;

            LoadMenuCommand = new RelayCommand(LoadMenu);
        }

        internal static MenuViewModel LoadMenus(MenuStore menuStore)
        {
            MenuViewModel menuViewModel = new MenuViewModel(menuStore);
            menuViewModel.LoadMenuCommand.Execute(null);
            return menuViewModel;
        }

        internal ICommand LoadMenuCommand { get; }
        internal IEnumerable<MenuStructure> Menus => _menuStore.Menus;


        #region commands
        private void LoadMenu(object parameter)
        {
            _menuStore.LoadMenu();
        }
        #endregion
    }
}
