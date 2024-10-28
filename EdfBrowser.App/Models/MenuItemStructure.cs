namespace EdfBrowser.App.Models
{
    public struct MenuItemStructure
    {
        private readonly string name; // name of the menu item

        public MenuItemStructure(string name)
        {
            this.name = name;
        }

        public string Description => name;
    }
}
