using System.Collections.Generic;

namespace EdfBrowser.App.Models
{
    public struct MenuStructure
    {
        private readonly string name; // name of the menu
        private readonly List<MenuItemStructure> menuItems;

        public MenuStructure(string name, List<MenuItemStructure> menuItems)
        {
            this.name = name;
            this.menuItems = menuItems;
        }

        public string Description => name;
        public IEnumerable<MenuItemStructure> MenuItems => menuItems;
    }
}
