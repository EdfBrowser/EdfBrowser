using EdfBrowser.Model;
using System.Collections.Generic;

namespace EdfBrowser.Services
{
    public interface IMenuService
    {
        List<MenuStructure> CreateMenuStructure(Dictionary<string, List<string>> menus_dict);
    }
}
