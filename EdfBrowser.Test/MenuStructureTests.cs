using EdfBrowser.Models;
using EdfBrowser.Services;
using NUnit.Framework;
using System.Collections.Generic;

namespace EdfBrowser.Test
{
    [TestFixture]
    public class MenuStructureTests
    {
        private IMenuService m_menuService;
        [SetUp]
        public void SetUp()
        {
            m_menuService = new MenuService();
        }

        // 测试创建菜单结构的功能
        [Test]
        public void CreateMenuStructure_HappyPath()
        {
            // Arrange
            var menus_dict = new Dictionary<string, List<string>>
            {
                { "File", new List<string> { "New", "Open", "Save" } },
                { "Edit", new List<string> { "Cut", "Copy", "Paste" } }
            };
            var expected = new List<MenuStructure>
            {
                new MenuStructure("File", new List<MenuItemStructure>
                {
                    new MenuItemStructure("New"),
                    new MenuItemStructure("Open"),
                    new MenuItemStructure("Save")
                }),
                new MenuStructure("Edit", new List<MenuItemStructure>
                {
                    new MenuItemStructure("Cut"),
                    new MenuItemStructure("Copy"),
                    new MenuItemStructure("Paste")
                })
            };

            // Act
            var result = m_menuService.CreateMenuStructure(menus_dict);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        // 测试传入空字典的边界情况
        [Test]
        public void CreateMenuStructure_EmptyDictionary()
        {
            // Arrange
            var menus_dict = new Dictionary<string, List<string>>();

            // Act
            var result = m_menuService.CreateMenuStructure(menus_dict);

            // Assert - 应该返回空列表
            Assert.AreEqual(0, result.Count);
        }

        // 测试传入null的边界情况
        [Test]
        public void CreateMenuStructure_NullDictionary()
        {
            // Act
            var result = m_menuService.CreateMenuStructure(null);

            // Assert - 应该返回空列表
            Assert.AreEqual(0, result.Count);
        }

        // 测试一个菜单没有子项的情况
        [Test]
        public void CreateMenuStructure_MenuWithoutItems()
        {
            // Arrange
            var menus_dict = new Dictionary<string, List<string>>
        {
            { "File", null } // 没有子项
        };
            var expected = new List<MenuStructure>
        {
            new MenuStructure("File", new List<MenuItemStructure>())
        };

            // Act
            var result = m_menuService.CreateMenuStructure(menus_dict);

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
