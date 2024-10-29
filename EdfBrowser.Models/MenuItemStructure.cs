using System;
using System.Diagnostics.CodeAnalysis;

namespace EdfBrowser.Models
{
    public struct MenuItemStructure
    {
        private readonly string m_name; // m_name of the menu item

        public MenuItemStructure(string name)
        {
            this.m_name = name;
        }

        public string Description => m_name;

        public override bool Equals(object obj)
        {
            if (obj is MenuItemStructure other)
            {
                return string.Equals(m_name, other.m_name, StringComparison.Ordinal);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return m_name != null ? m_name.GetHashCode() : 0;
        }
    }
}
