using EdfBrowser.App.Models;
using System.Collections.Generic;

namespace EdfBrowser.App.Services
{
    public class MenuService
    {
        /// <summary>
        /// 
        /// </summary>
        public List<MenuStructure> CreateMenuStructure(Dictionary<string, List<string>> menus_dict)
        {
            List<MenuStructure> menus = new List<MenuStructure>();

            if (menus_dict == null || menus_dict.Count == 0) return menus; // No Menus to create

            // Create Menus and Menu Items
            foreach (var menu in menus_dict)
            {
                List<MenuItemStructure> menu_items = new List<MenuItemStructure>();
                if (menu.Value != null)
                {
                    foreach (var item in menu.Value)
                    {
                        MenuItemStructure menu_item = new MenuItemStructure(item);
                        menu_items.Add(menu_item);
                    }
                }

                MenuStructure file_menu = new MenuStructure(menu.Key, menu_items);
                menus.Add(file_menu);
            }

            return menus;
        }
    }
}
