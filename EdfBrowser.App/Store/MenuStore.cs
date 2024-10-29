using EdfBrowser.Models;
using EdfBrowser.Services;
using System;
using System.Collections.Generic;

namespace EdfBrowser.App.Store
{
    public class MenuStore
    {
        public event EventHandler<List<MenuStructure>> MenuChangedEventHandler;

        private readonly Dictionary<string, List<string>> mock_dict = new Dictionary<string, List<string>>()
        {
            {"File", new List<string> (){ "Open", "Close" } },
        };

        private readonly MenuService m_menuService;

        public MenuStore(MenuService menuService)
        {
            m_menuService = menuService;
        }

        public List<MenuStructure> Menus { get; set; }

        public void LoadMenu()
        {
            Menus = m_menuService.CreateMenuStructure(mock_dict);

            MenuChangedEventHandler?.Invoke(this, Menus);
        }
    }
}
