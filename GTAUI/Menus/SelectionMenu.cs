using GTAUI.Menus.MenuItems;
using GTAUI.Styling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTAUI.Menus
{
    /// <summary>
    /// A menu that allows the user to pick an option.
    /// The menu can be further customized by inheriting from this class.
    /// </summary>
    /// <typeparam name="T">The type of object that can be selected.</typeparam>
    class SelectionMenu<T> : Menu
    {
        private UIStyle uiStyle = UIStyle.GetInstance();
        private readonly IEnumerable<T> options;
        private readonly Action<T> onSelection;
        private readonly Action onCancel;
        private bool itemHasBeenSelected = false;

        /// <summary>
        /// <c>true</c> when the last menu option should be a button to close the menu.
        /// If both <see cref="ShowCloseButton"/> and <see cref="ShowBackButton"/> are <c>true</c> only the close button will be shown.
        /// The default value of this property is determined by the <c>gtaui.menus.selectionMenu.defaults.showCloseButton</c> UIStyle property.
        /// </summary>
        public bool ShowCloseButton { get; set; }

        /// <summary>
        /// <c>true</c> when the last menu option should be a button to go back to the parent menu of this menu.
        /// If both <see cref="ShowCloseButton"/> and <see cref="ShowBackButton"/> are <c>true</c> only the close button will be shown.
        /// The default value of this property is determined by the <c>gtaui.menus.selectionMenu.defaults.showBackButton</c> UIStyle property.
        /// </summary>
        public bool ShowBackButton { get; set; }

        /// <summary>
        /// <c>true</c> if disabled options should not be shown in the menu.
        /// The default value of this property is determined by the <c>gtaui.menus.selectionMenu.defaults.hideDisabledOptions</c> UIStyle property.
        /// </summary>
        public bool HideDisabledOptions { get; set; }

        /// <summary>
        /// A reference to a function to check wether an option is enabled. if this is <c>null</c>, all options will be enabled.
        /// The default value is a reference to <see cref="OptionShouldBeEnabled(T)"/> which can be overrridden in a derived class.
        /// </summary>
        public Func<T, bool> OptionShouldBeEnabledFunc { get; set; }

        private SelectionMenu() : base(string.Empty) { }

        /// <summary>
        /// Create a new selection menu.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="options">The options to show in the menu.</param>
        /// <param name="onSelection">The action to invoke when a selection is made by the user.</param>
        public SelectionMenu(string title, IEnumerable<T> options, Action<T> onSelection) : this(title, null, options, onSelection) { }

        /// <summary>
        /// Create a new selection menu.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="options">The options to show in the menu.</param>
        /// <param name="onSelection">The action to invoke when a selection is made by the user.</param>
        /// <param name="onCancel">The action to invoke when the user closes the menu without choosing an option.</param>
        public SelectionMenu(string title, IEnumerable<T> options, Action<T> onSelection, Action onCancel) : this(title, null, options, onSelection, onCancel) { }

        /// <summary>
        /// Create a new selection menu.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="subtitle">The subtitle of the menu.</param>
        /// <param name="options">The options to show in the menu.</param>
        /// <param name="onSelection">The action to invoke when a selection is made by the user.</param>
        public SelectionMenu(string title, string subtitle, IEnumerable<T> options, Action<T> onSelection) : this(title, subtitle, options, onSelection, null) { }

        /// <summary>
        /// Create a new selection menu.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="subtitle">The subtitle of the menu.</param>
        /// <param name="options">The options to show in the menu.</param>
        /// <param name="onSelection">The action to invoke when a selection is made by the user.</param>
        /// <param name="onCancel">The action to invoke when the user closes the menu without choosing an option.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionMenu(string title, string subtitle, IEnumerable<T> options, Action<T> onSelection, Action onCancel) : base(title, subtitle)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.onSelection = onSelection ?? throw new ArgumentNullException(nameof(onSelection));
            this.onCancel = onCancel;

            OptionShouldBeEnabledFunc = OptionShouldBeEnabled;
            ShowCloseButton = uiStyle.GetStyleProperty<bool>("gtaui.menus.selectionMenu.defaults.showCloseButton");
            ShowBackButton = uiStyle.GetStyleProperty<bool>("gtaui.menus.selectionMenu.defaults.showBackButton");
            HideDisabledOptions = uiStyle.GetStyleProperty<bool>("gtaui.menus.selectionMenu.defaults.hideDisabledOptions");
            MenuInstance.Closed += MenuClosed;
        }

        private void MenuClosed(object sender, EventArgs e)
        {
            if (itemHasBeenSelected == false)
            {
                onCancel?.Invoke();
                itemHasBeenSelected = false;
            }
        }

        /// <summary>
        /// Create a menu item for the given option.
        /// The title and description do not need to be set.
        /// If you want to use the default behaviour when activating the menu item, pass the <see cref="OptionMenuItemActivated"/> method as the onActivated action.
        /// </summary>
        /// <param name="option">The option to create a menu item for.</param>
        /// <returns>A menu item for the given option.</returns>
        protected virtual MenuItem CreateMenuItemForOption(T option)
        {

            return new ButtonMenuItem(string.Empty, string.Empty, OptionMenuItemActivated);
        }

        /// <summary>
        /// Create a menu item that acts as a close button.
        /// Activating this menu item should call <see cref="Menu.Close"/>.
        /// </summary>
        /// <returns>A menu item that acts as a close button.</returns>
        protected virtual MenuItem CreateCloseButton()
        {
            CloseMenuItem item = new CloseMenuItem();
            item.Title = uiStyle.GetStyleProperty<string>("gtaui.menus.selectionMenu.closeMenuItem.title");
            item.Description = uiStyle.GetStyleProperty<string>("gtaui.menus.selectionMenu.closeMenuItem.description");

            return item;
        }

        /// <summary>
        /// Create a menu item that acts as a back button.
        /// Activating this menu item shoud call <see cref="Menu.Back"/>.
        /// </summary>
        /// <returns>A menu item that acts as a back button.</returns>
        protected virtual MenuItem CreateBackButton()
        {
            BackMenuItem item = new BackMenuItem();
            item.Title = uiStyle.GetStyleProperty<string>("gtaui.menus.selectionMenu.backMenuItem.title");
            item.Description = uiStyle.GetStyleProperty<string>("gtaui.menus.selectionMenu.backMenuItem.description");

            return item;
        }

        /// <summary>
        /// Build the selection menu before showing it.
        /// </summary>
        /// <returns><c>true</c> if the menu is valid and will be shown. <c>false</c> otherwise.</returns>
        protected override bool BeforeShow()
        {
            if (options.Any() == false)
            {
                UIController.Log($"Warning: Unable to show selection menu with title '{Title}' because it has no items.");
                return false;
            }

            ClearMenuItems();

            foreach(T item in options)
            {
                string title = item is IMenuSelectable ? (item as IMenuSelectable).GetMenuItemTitle() : item.ToString();
                string description = item is IMenuSelectable ? (item as IMenuSelectable).GetMenuItemDescription() : string.Empty;
                bool enabled = true;
                if (OptionShouldBeEnabledFunc != null)
                {
                    enabled = item is IMenuSelectable ? (item as IMenuSelectable).IsMenuItemEnabled() : OptionShouldBeEnabledFunc.Invoke(item);
                }
                else
                {
                    enabled = item is IMenuSelectable ? (item as IMenuSelectable).IsMenuItemEnabled() : true;
                }


                if (HideDisabledOptions && enabled == false)
                {
                    continue;
                }

                MenuItem menuItem = CreateMenuItemForOption(item);
                if (menuItem == null)
                {
                    UIController.Log($"Error: CreateMenuItemForOption returned null when creating menu items for a selection menu with title '{Title}'.");
                    return false;
                }

                menuItem.Title = title;
                menuItem.Description = description;
                menuItem.Item.Enabled = enabled;
                menuItem.Item.Tag = item;

                AddMenuItem(menuItem);
            }

            if (ShowCloseButton)
            {
                AddMenuItem(CreateCloseButton());
            }
            else if (ShowBackButton)
            {
                AddMenuItem(CreateBackButton());
            }

            return true;
        }

        /// <summary>
        /// Returns wether the given option should be enabled or not.
        /// Note: this method will only be called when <see cref="OptionShouldBeEnabledFunc"/> has it's default value.
        /// </summary>
        /// <param name="option">The option to check.</param>
        /// <returns><c>true</c> if the option should be enabled. <c>false</c> otherwise.</returns>
        protected virtual bool OptionShouldBeEnabled(T option)
        {
            return true;
        }

        /// <summary>
        /// Default handler method for when an option menu item is activated.
        /// </summary>
        /// <param name="item">The activated menu item.</param>
        protected void OptionMenuItemActivated(MenuItem item)
        {
            if (item.Item.Tag == null || item.Item.Tag is T == false)
            {
                UIController.Log($"Error: option menu item '{item.Title}' was selected but it's Tag property does not contain valid data!");
                return;
            }
            itemHasBeenSelected = true;
            onSelection((T)item.Item.Tag);
        }

    }
}
