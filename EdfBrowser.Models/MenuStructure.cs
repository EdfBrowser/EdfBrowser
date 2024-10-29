using System;
using System.Collections.Generic;
using System.Linq;

namespace EdfBrowser.Models
{
    public struct MenuStructure
    {
        private readonly string m_name; // m_name of the menu
        private readonly List<MenuItemStructure> m_menuItems;

        public MenuStructure(string name, List<MenuItemStructure> menuItems)
        {
            this.m_name = name;
            this.m_menuItems = menuItems;
        }

        public string Description => m_name;
        public IEnumerable<MenuItemStructure> MenuItems => m_menuItems;

        public override bool Equals(object obj)
        {
            if (obj is MenuStructure other)
            {
                return string.Equals(m_name, other.m_name, StringComparison.Ordinal)
                    && m_menuItems.SequenceEqual(other.m_menuItems); // 比较菜单项
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = m_name != null ? m_name.GetHashCode() : 0;
            foreach (var item in m_menuItems)
            {
                hash = hash * 31 + item.GetHashCode(); // 使用一个质数来组合哈希值
            }
            return hash;
        }
    }
}
