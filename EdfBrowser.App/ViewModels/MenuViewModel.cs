using EdfBrowser.App.Commands;
using EdfBrowser.App.Models;
using EdfBrowser.App.Services;
using EdfBrowser.App.Store;
using EdfBrowser.App.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EdfBrowser.App.ViewModels
{
    public class MenuViewModel
    {
        private List<MenuStructure> m_menus;

        private readonly MenuStore m_menuStore;

        public MenuViewModel(MenuStore menuStore)
        {
            m_menuStore = menuStore;
            m_menuStore.MenuChangedEventHandler += OnMenuChanged;
            LoadMenuCommand = new RelayCommand(LoadMenu);
        }

        public ICommand LoadMenuCommand { get; }
        public IEnumerable<MenuStructure> Menus
        {
            get
            {
                return m_menus;
            }
            set
            {
                m_menus = m_menuStore.Menus;
                // 通知ui更新
            }
        }

        private void OnMenuChanged(object sender, List<MenuStructure> e)
        {
            Menus = e;
        }

        #region commands
        private void LoadMenu(object parameter)
        {
            m_menuStore.LoadMenu();
        }
        #endregion
    }
}
