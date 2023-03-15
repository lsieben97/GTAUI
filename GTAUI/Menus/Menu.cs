using GTAUI.Menus.MenuItems;
using LemonUI.Menus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.Menus
{
    /// <summary>
    /// Represents a menu controlled by the <see cref="UIController"/>.
    /// This class wraps around <see cref="NativeMenu"/> and allows menu's to be instanciated through Json.
    /// </summary>
    public class Menu
    {
        /// <summary>
        /// The UI resource this menu was loaded from. <c>null</c> if the menu was not loaded from a UI resource.
        /// </summary>
        public string UIResourcePath { get; } = null;

        /// <summary>
        /// The title of the menu.
        /// This property is directly connected to the <see cref="NativeMenu.Title"/> property's <see cref="LemonUI.Elements.ScaledText.Text"/> property.
        /// </summary>
        public string Title { get => MenuInstance.Title.Text; set => MenuInstance.Title.Text = value; }

        /// <summary>
        /// The subtitle of the menu.
        /// This property is directly connected to the <see cref="NativeMenu.Subtitle"/> property.
        /// </summary>
        public string SubTitle { get => MenuInstance.Subtitle; set => MenuInstance.Subtitle = value; }

        /// <summary>
        /// The actual <see cref="NativeMenu"/> that will be displayed.
        /// Directly modifying the menu items through this property is not recommended as it can 'de-sync' the actual contents with the <see cref="Items"/> property.
        /// </summary>
        public NativeMenu MenuInstance { get; private set; }

        /// <summary>
        /// The items that are in this menu.
        /// </summary>
        public List<MenuItem> Items { get; private set; } = new List<MenuItem>();


        /// <summary>
        /// Create a new menu based on the json definition in the UI resource at the given <paramref name="uiResourcePath"/>.
        /// </summary>
        /// <param name="uiResourcePath">The path of the UI resource that contains the json definition to use to build the menu.</param>
        public Menu(string uiResourcePath)
        {
            UIResourcePath = uiResourcePath;

            InitializeFromUIResource();
        }

        /// <summary>
        /// Create a new menu without any menu items.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="subTitle">The sub title of the menu.</param>
        /// <exception cref="NullReferenceException">When <paramref name="title"/> is null.</exception>
        public Menu(string title, string subTitle = "")
        {
            if (title is null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            MenuInstance = new NativeMenu(title, subTitle);
            Items = new List<MenuItem>();
        }

        private void InitializeFromUIResource()
        {
            string menuJson = UIController.instance.GetUIResource(UIResourcePath);
            if (menuJson == null)
            {
                string message = $"Unable to load menu {GetType()}: UI resource not found: {UIResourcePath}";
                UIController.Log(message);
                throw new Exception(message);
            }

            JsonMenu jsonMenu = null;

            try
            {
                jsonMenu = JsonConvert.DeserializeObject<JsonMenu>(menuJson);
            }
            catch (Exception ex)
            {
                UIController.Log($"Could not load menu resource {UIResourcePath}: {ex}");
            }

            if (jsonMenu == null)
            {
                string message = $"Unable to load menu {GetType()}: Unable to parse json from {UIResourcePath}";
                UIController.Log(message);
                throw new Exception(message);
            }

            MenuInstance = new NativeMenu(string.Empty);

            Title = jsonMenu.Title != null ? jsonMenu.Title : "Menu";
            SubTitle = jsonMenu.SubTitle != null ? jsonMenu.SubTitle : string.Empty;

            if (jsonMenu.Items == null)
            {
                jsonMenu.Items = new List<MenuItem>();
            }

            InitializeMenuItems(jsonMenu.Items);

            InitializeMenuInstance();
        }

        private void InitializeMenuItems(List<MenuItem> menuItems)
        {
            if (menuItems is null)
            {
                throw new ArgumentNullException(nameof(menuItems));
            }

            foreach (MenuItem item in menuItems)
            {
                item.InitializeFromExistingProperties(this, GetType());
            }

            Items.AddRange(menuItems);

            if (Items.Any() == false)
            {
                UIController.Log($"Warning: menu {GetType()} does not have any menu items!");
            }
        }

        private void InitializeMenuInstance()
        {
            List<MenuItem> itemsToRemove = new List<MenuItem>();
            foreach (MenuItem item in Items)
            {
                if (item.IsValid == false)
                {
                    itemsToRemove.Add(item);
                    continue;
                }
                MenuInstance.Add(item.Item);
            }

            foreach (MenuItem item in itemsToRemove)
            {
                Items.Remove(item);
            }
        }


        /// <summary>
        /// Show the menu on screen.
        /// </summary>
        public void Show()
        {
            if (BeforeShow() == false)
            {
                return;
            }

            if (MenuInstance.Visible == false)
            {
                UIController.instance.ShowMenu(this);
            }
        }

        /// <summary>
        /// Hide the menu.
        /// This also stops the <see cref="UIController"/> from managing this menu.
        /// Call <see cref="Show"/> to let the <see cref="UIController"/> manage this menu again.
        /// </summary>
        public void Close()
        {
            if (MenuInstance.Visible == false)
            {
                return;
            }

            MenuInstance.Visible = false;
            UIController.instance.RemoveMenu(this);
        }

        /// <summary>
        /// Hide this menu and display the parent menu if there is any.
        /// If this menu does not have a parent menu calling this method does the same as calling <see cref="Close"/>.
        /// 
        /// This also stops the <see cref="UIController"/> from managing this menu.
        /// Call <see cref="Show"/> to let the <see cref="UIController"/> manage this menu again.
        /// </summary>
        public void Back()
        {
            if (MenuInstance.Parent == null)
            {
                Close();
            }

            if (MenuInstance.Visible == false)
            {
                return;
            }
            
            MenuInstance.Parent.Visible = true;
            MenuInstance.Back();
            UIController.instance.RemoveMenu(this);
        }

        /// <summary>
        /// Get the <see cref="MenuItem"/> with the given <paramref name="id"/> or <c>null</c> if there is no menu item with the given id.
        /// </summary>
        /// <typeparam name="T">The tyoe of menu item.</typeparam>
        /// <param name="id">The id to search for.</param>
        /// <returns>The <see cref="MenuItem"/> with the given <paramref name="id"/> or <c>null</c> if there is no menu item with the given id.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public T GetMenuItem<T>(string id) where T : MenuItem
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return Items.Find(i => i.Id == id) as T;
        }

        /// <summary>
        /// Add the given <paramref name="item"/> to the menu.
        /// Will check the <see cref="MenuItem.IsValid"/> property to see if the menu item can be added.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <exception cref="NullReferenceException"></exception>
        public void AddMenuItem(MenuItem item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (item.IsValid == false)
            {
                return;
            }

            Items.Add(item);
            item.ParentMenu = this;
            MenuInstance.Add(item.Item);
        }

        /// <summary>
        /// Set the given menu as this menu's parent manu.
        /// If this menu's <see cref="Back"/> method is called the given menu is shown after this menu is hidden.
        /// </summary>
        /// <seealso cref="Back"/>
        /// <param name="parentMenu"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void SetParentMenu(Menu parentMenu)
        {
            if (parentMenu is null)
            {
                throw new ArgumentNullException(nameof(parentMenu));
            }

            MenuInstance.Parent = parentMenu.MenuInstance;
        }

        /// <summary>
        /// Set the given menu as this menu's parent manu.
        /// If this menu's <see cref="Back"/> method is called the given menu is shown after this menu is hidden.
        /// </summary>
        /// <param name="parentMenu"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void SetParentMenu(NativeMenu parentMenu)
        {
            if (parentMenu is null)
            {
                throw new ArgumentNullException(nameof(parentMenu));
            }

            MenuInstance.Parent = parentMenu;
        }

        /// <summary>
        /// Called before the menu is shown.
        /// Usefull to lazy load menu items and such.
        /// </summary>
        /// <returns><c>true</c> if the menu should be shown. <c>false</c> otherwise.</returns>
        protected virtual bool BeforeShow() => true;

        /// <summary>
        /// Remove all <see cref="MenuItem"/>s from this menu.
        /// </summary>
        public void ClearMenuItems()
        {
            Items.Clear();
            MenuInstance.Clear();
        }
    }
}
