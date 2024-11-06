using EdfBrowser.App.Commands;
using EdfBrowser.App.Store;
using EdfBrowser.Models;
using System.Collections.Generic;
using System.Windows.Input;

namespace EdfBrowser.App.ViewModels
{
    public class MenuViewModel
    {
        private readonly EdfDashBoardViewModel m_edfDashBoardViewModel;

        private readonly MenuStore m_menuStore;

        private MenuViewModel(MenuStore menuStore, EdfDashBoardViewModel edfDashBoardViewModel)
        {
            m_menuStore = menuStore;
            m_edfDashBoardViewModel = edfDashBoardViewModel;

            LoadMenuCommand = new RelayCommand(LoadMenu);
        }

        public static MenuViewModel LoadMenus(MenuStore menuStore, EdfDashBoardViewModel edfDashBoardViewModel)
        {
            MenuViewModel menuViewModel = new MenuViewModel(menuStore, edfDashBoardViewModel);
            menuViewModel.LoadMenuCommand.Execute(null);
            return menuViewModel;
        }

        public ICommand LoadMenuCommand { get; }
        public IEnumerable<MenuStructure> Menus => m_menuStore.Menus;

        public EdfDashBoardViewModel EdfDashBoardViewModel => m_edfDashBoardViewModel;

        #region commands
        private void LoadMenu(object parameter)
        {
            m_menuStore.LoadMenu();
        }
        #endregion
    }
}
