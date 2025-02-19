using System;

namespace EdfBrowser.Models
{
    public readonly struct MenuItemStructure
    {
        private readonly string _name; // _name of the menu item

        public MenuItemStructure(string name)
        {
            this._name = name;
        }

        public string Description => _name;

        public override bool Equals(object obj)
        {
            if (obj is MenuItemStructure other)
            {
                return string.Equals(_name, other._name, StringComparison.Ordinal);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _name != null ? _name.GetHashCode() : 0;
        }
    }
}
