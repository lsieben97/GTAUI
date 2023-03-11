using GTAUI.Menus.MenuItems;
using LemonUI.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.Menus
{
    /// <summary>
    /// Builder class to build <see cref="Menu"/>s in a fluent way.
    /// </summary>
    public class MenuBuilder
    {
        private Menu menu;

        /// <summary>
        /// Create a new builder to build a menu with an empty title and description.
        /// </summary>
        public MenuBuilder() :this(string.Empty, string.Empty) { }

        /// <summary>
        /// Create a new builder to build a menu with the given title.
        /// </summary>
        /// <param name="title">The title of the menu that this builder will build.</param>
        public MenuBuilder(string title) : this(title, string.Empty) { }

        /// <summary>
        /// Create a new builder to build a menu with the given title and sub title.
        /// </summary>
        /// <param name="title">The title of the menu that this builder will build.</param>
        /// <param name="subTitle">The title of the menu that this builder will build.</param>
        public MenuBuilder(string title, string subTitle)
        {
            if (title is null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            menu = new Menu(title, subTitle ?? string.Empty);
        }

        /// <summary>
        /// Return the menu that has been built.
        /// </summary>
        /// <returns>The menu that has been built.</returns>
        public Menu Build()
        {
            return menu;
        }

        /// <summary>
        /// Set the title of the menu.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder WithTitle(string title)
        {
            menu.Title = title;
            return this;
        }

        /// <summary>
        /// Set the sub title of the menu.
        /// </summary>
        /// <param name="subTitle">The sub title of the menu.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder WithSubTitle(string subTitle)
        {
            menu.SubTitle = subTitle;
            return this;
        }

        /// <summary>
        /// Set the parent menu of the menu being built.
        /// </summary>
        /// <param name="parentMenu">The parent menu of the menu being built.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder WithParentMenu(Menu parentMenu)
        {
            menu.SetParentMenu(parentMenu);
            return this;
        }

        /// <summary>
        /// Set the parent menu of the menu being built.
        /// </summary>
        /// <param name="parentMenu">The parent menu of the menu being built.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder WithParent(NativeMenu parentMenu)
        {
            menu.SetParentMenu(parentMenu);
            return this;
        }

        /// <summary>
        /// Add a new <see cref="ButtonMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="onActivated">The action to invoke when the menu item is activated.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddButton(string id, string title, Action<MenuItem> onActivated)
        {
            return AddButton(id, title, string.Empty, onActivated);
        }

        /// <summary>
        /// Add a new <see cref="ButtonMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="onActivated">The action to invoke when the menu item is activated.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddButton(string id, string title, string description, Action<MenuItem> onActivated)
        {
            return AddButton(id, title, description, onActivated, null);
        }

        /// <summary>
        /// Add a new <see cref="ButtonMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="onActivated">The action to invoke when this menu item is activated.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddButton(string id, string title, Action<MenuItem> onActivated, Action<MenuItem> onSelected)
        {
            return AddButton(id, title, string.Empty, onActivated, onSelected);
        }

        /// <summary>
        /// Add a new <see cref="ButtonMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="onActivated">The action to invoke when this menu item is activated.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddButton(string id, string title, string description, Action<MenuItem> onActivated, Action<MenuItem> onSelected)
        {
            menu.AddMenuItem(new ButtonMenuItem(id, title, description, onSelected, onActivated));
            return this;
        }

        /// <summary>
        /// Add a new <see cref="CheckBoxMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="onCheckboxChanged">The action to invoke when the state of the checkbox changes.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddCheckbox(string id, string title, Action<MenuItem, bool> onCheckboxChanged)
        {
            return AddCheckbox(id, title, string.Empty, onCheckboxChanged);
        }

        /// <summary>
        /// Add a new <see cref="CheckBoxMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="onCheckboxChanged">The action to invoke when the state of the checkbox changes.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddCheckbox(string id, string title, string description, Action<MenuItem, bool> onCheckboxChanged)
        {
            return AddCheckbox(id, title, description, onCheckboxChanged, null);
        }

        /// <summary>
        /// Add a new <see cref="CheckBoxMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="onCheckboxChanged">The action to invoke when the state of the checkbox changes.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddCheckbox(string id, string title, Action<MenuItem, bool> onCheckboxChanged, Action<MenuItem> onSelected)
        {
            return AddCheckbox(id, title, string.Empty, onCheckboxChanged, onSelected);
        }

        /// <summary>
        /// Add a new <see cref="CheckBoxMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="onCheckboxChanged">The action to invoke when the state of the checkbox changes.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddCheckbox(string id, string title, string description, Action<MenuItem, bool> onCheckboxChanged, Action<MenuItem> onSelected)
        {
            menu.AddMenuItem(new CheckBoxMenuItem(id, title, description, onCheckboxChanged, onSelected));
            return this;
        }

        /// <summary>
        /// Add a new <see cref="ListMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="list">The list of values to show.</param>
        /// <param name="onSelectionChanged"></param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddList(string id, string title, IEnumerable<object> list, Action<MenuItem, object> onSelectionChanged)
        {
            return AddList(id, title, string.Empty, list, onSelectionChanged);
        }

        /// <summary>
        /// Add a new <see cref="ListMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description"></param>
        /// <param name="list">The list of values to show.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddList(string id, string title, string description, IEnumerable<object> list, Action<MenuItem, object> onSelectionChanged)
        {
            return AddList(id, title, description, list, onSelectionChanged, null);
        }

        /// <summary>
        /// Add a new <see cref="ListMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="list">The list of values to show.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddList(string id, string title, IEnumerable<object> list, Action<MenuItem, object> onSelectionChanged, Action<MenuItem> onSelected)
        {
            return AddList(id, title, string.Empty, list, onSelectionChanged, onSelected);
        }

        /// <summary>
        /// Add a new <see cref="ListMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description"></param>
        /// <param name="list">The list of values to show.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddList(string id, string title, string description, IEnumerable<object> list, Action<MenuItem, object> onSelectionChanged, Action<MenuItem> onSelected)
        {
            menu.AddMenuItem(new ListMenuItem(id, title, description, list, onSelectionChanged, onSelected));
            return this;
        }

        /// <summary>
        /// Add a new <see cref="SliderMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="maximum">The maximum value the user can choose.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddSlider(string id, string title, int maximum, Action<MenuItem, int> onSelectionChanged)
        {
            return AddSlider(id, title, string.Empty, maximum, onSelectionChanged);
        }

        /// <summary>
        /// Add a new <see cref="SliderMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="maximum">The maximum value the user can choose.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddSlider(string id, string title, string description, int maximum, Action<MenuItem, int> onSelectionChanged)
        {
            return AddSlider(id, title, description, maximum, onSelectionChanged, null);
        }

        /// <summary>
        /// Add a new <see cref="SliderMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="maximum">The maximum value the user can choose.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddSlider(string id, string title, int maximum, Action<MenuItem, int> onSelectionChanged, Action<MenuItem> onSelected)
        {
            return AddSlider(id, title, string.Empty, maximum, onSelectionChanged, onSelected);
        }

        /// <summary>
        /// Add a new <see cref="SliderMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="maximum">The maximum value the user can choose.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddSlider(string id, string title, string description, int maximum, Action<MenuItem, int> onSelectionChanged, Action<MenuItem> onSelected)
        {
            menu.AddMenuItem(new SliderMenuItem(id, title, description, maximum, onSelectionChanged, onSelected));
            return this;
        }

        /// <summary>
        /// Add a new <see cref="SubMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="subMenu">The menu to show when this menu item is activated.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddSubMenu(string id, string title, Menu subMenu)
        {
            return AddSubMenu(id, title, null, subMenu);
        }

        /// <summary>
        /// Add a new <see cref="SubMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="subMenu">The menu to show when this menu item is activated.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddSubMenu(string id, string title, string description, Menu subMenu)
        {
            return AddSubMenu(id, title, description, subMenu, null);
        }

        /// <summary>
        /// Add a new <see cref="SubMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="subMenu">The menu to show when this menu item is activated.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddSubMenu(string id, string title, Menu subMenu, Action<MenuItem> onSelected)
        {
            return AddSubMenu(id, title, null, subMenu, onSelected);
        }

        /// <summary>
        /// Add a new <see cref="SubMenuItem"/> to the menu.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="subMenu">The menu to show when this menu item is activated.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddSubMenu(string id, string title, string description, Menu subMenu, Action<MenuItem> onSelected)
        {
            menu.AddMenuItem(new SubMenuItem(id, title, description, subMenu, onSelected));
            return this;
        }

        /// <summary>
        /// Add a new <see cref="CloseMenuItem"/> to the menu.
        /// </summary>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddCloseMenuItem()
        {
            return AddCloseMenuItem(null);
        }

        /// <summary>
        /// Add a new <see cref="CloseMenuItem"/> to the menu.
        /// </summary>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddCloseMenuItem(Action<MenuItem> onSelected)
        {
            menu.AddMenuItem(new CloseMenuItem(onSelected));
            return this;
        }

        /// <summary>
        /// Add a new <see cref="BackMenuItem"/> to the menu.
        /// </summary>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddBackMenuItem()
        {
            return AddBackMenuItem(null);
        }

        /// <summary>
        /// Add a new <see cref="BackMenuItem"/> to the menu.
        /// </summary>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <returns>The builder so you can continue building.</returns>
        public MenuBuilder AddBackMenuItem(Action<MenuItem> onSelected)
        {
            menu.AddMenuItem(new BackMenuItem(onSelected));
            return this;
        }
    }
}
