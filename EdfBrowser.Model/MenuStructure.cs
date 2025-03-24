using System;
using System.Collections.Generic;
using System.Linq;

namespace EdfBrowser.Model
{
    public readonly struct MenuStructure
    {
        private readonly string _name; // _name of the menu
        private readonly List<MenuItemStructure> _menuItems;

        public MenuStructure(string name, List<MenuItemStructure> menuItems)
        {
            this._name = name;
            this._menuItems = menuItems;
        }

        public string Description => _name;
        public IEnumerable<MenuItemStructure> MenuItems => _menuItems;

        public override bool Equals(object obj)
        {
            if (obj is MenuStructure other)
            {
                return string.Equals(_name, other._name, StringComparison.Ordinal)
                    && _menuItems.SequenceEqual(other._menuItems); // 比较菜单项
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = _name != null ? _name.GetHashCode() : 0;
            foreach (var item in _menuItems)
            {
                hash = hash * 31 + item.GetHashCode(); // 使用一个质数来组合哈希值
            }
            return hash;
        }
    }
}
