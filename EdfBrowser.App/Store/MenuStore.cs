using EdfBrowser.Models;
using EdfBrowser.Services;
using System.Collections.Generic;

namespace EdfBrowser.App
{
    internal class MenuStore
    {
        private readonly Dictionary<string, List<string>> _mocks = new Dictionary<string, List<string>>()
        {
            {"File", new List<string> (){ "Open", "Close" } },
        };

        private readonly IMenuService _menuService;

        internal MenuStore(IMenuService menuService)
        {
            _menuService = menuService;
        }

        internal IEnumerable<MenuStructure> Menus { get; private set; }

        internal void LoadMenu()
        {
            Menus = _menuService.CreateMenuStructure(_mocks);
        }
    }
}
