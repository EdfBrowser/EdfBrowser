using EdfBrowser.Models;
using EdfBrowser.Services;
using System.Collections.Generic;

namespace EdfBrowser.App.Store
{
    public class MenuStore
    {
        private readonly Dictionary<string, List<string>> mock_dict = new Dictionary<string, List<string>>()
        {
            {"File", new List<string> (){ "Open", "Close" } },
        };

        private readonly IMenuService m_menuService;

        public MenuStore(IMenuService menuService)
        {
            m_menuService = menuService;
        }

        public IEnumerable<MenuStructure> Menus { get; private set; }

        public void LoadMenu()
        {
            Menus = m_menuService.CreateMenuStructure(mock_dict);
        }
    }
}
