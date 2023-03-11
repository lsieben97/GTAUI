using System;
using GTAUI.Styling;
using System.Collections.Generic;
using GTAUI.Menus.MenuItems;
using LemonUI.Menus;
using System.Linq;

namespace GTAUI.Menus
{
    /// <summary>
    /// A menu that allows the user to pick multiple options.
    /// The menu can be further customized by inheriting from this class.
    /// </summary>
    /// <typeparam name="T">The type of object that can be selected.</typeparam>
    public class MultiSelectionMenu<T> : Menu
    {
        private readonly UIStyle uiStyle = UIStyle.GetInstance();
        private readonly IEnumerable<T> options;
        private readonly Action<IEnumerable<T>> onSelection;
        private readonly Action onCancel;
        private readonly List<T> selectedOptions = new List<T>();
        private bool itemsHaveBeenSelected = false;
        private bool massUpdateHappening = false;
        private MenuItem acceptMenuItem = null;

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
        /// <c>true</c> when the menu should show a 'select all' menu item. <c>false</c> otherwise.
        /// </summary>
        public bool ShowSelectAllButton { get; set; }

        /// <summary>
        /// <c>true</c> when the menu should show a 'deselect all' menu item. <c>false</c> otherwise.
        /// </summary>
        public bool ShowDeselectAllButton { get; set; }

        /// <summary>
        /// A reference to a function to check whether an option is enabled. if this is <c>null</c>, all options will be enabled.
        /// The default value is a reference to <see cref="OptionShouldBeEnabled(T)"/> which can be overridden in a derived class.
        /// </summary>
        public Func<T, bool> OptionShouldBeEnabledFunc { get; set; }

        /// <summary>
        /// The action to invoke when the user changes the selected items.
        /// The <see cref="IEnumerable{T}"/> contains the new selection.
        /// </summary>
        public Action<IEnumerable<T>> OnSelectionChanged { get; set; } = null;


        private MultiSelectionMenu() : base(string.Empty) { }

        /// <summary>
        /// Create a new selection menu.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="options">The options to show in the menu.</param>
        /// <param name="onSelection">The action to invoke when a selection is made by the user.</param>
        public MultiSelectionMenu(string title, IEnumerable<T> options, Action<IEnumerable<T>> onSelection) : this(title, null, options, onSelection) { }

        /// <summary>
        /// Create a new selection menu.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="options">The options to show in the menu.</param>
        /// <param name="onSelection">The action to invoke when a selection is made by the user.</param>
        /// <param name="onCancel">The action to invoke when the user closes the menu without choosing an option.</param>
        public MultiSelectionMenu(string title, IEnumerable<T> options, Action<IEnumerable<T>> onSelection, Action onCancel) : this(title, null, options, onSelection, onCancel) { }

        /// <summary>
        /// Create a new selection menu.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="subtitle">The subtitle of the menu.</param>
        /// <param name="options">The options to show in the menu.</param>
        /// <param name="onSelection">The action to invoke when a selection is made by the user.</param>
        public MultiSelectionMenu(string title, string subtitle, IEnumerable<T> options, Action<IEnumerable<T>> onSelection) : this(title, subtitle, options, onSelection, null) { }

        /// <summary>
        /// Create a new selection menu.
        /// </summary>
        /// <param name="title">The title of the menu.</param>
        /// <param name="subtitle">The subtitle of the menu.</param>
        /// <param name="options">The options to show in the menu.</param>
        /// <param name="onSelection">The action to invoke when a selection is made by the user.</param>
        /// <param name="onCancel">The action to invoke when the user closes the menu without choosing an option.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MultiSelectionMenu(string title, string subtitle, IEnumerable<T> options, Action<IEnumerable<T>> onSelection, Action onCancel) : base(title, subtitle)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            this.onSelection = onSelection ?? throw new ArgumentNullException(nameof(onSelection));
            this.onCancel = onCancel;

            OptionShouldBeEnabledFunc = OptionShouldBeEnabled;
            ShowCloseButton = uiStyle.GetStyleProperty<bool>("gtaui.menus.multiSelectionMenu.defaults.showCloseButton");
            ShowBackButton = uiStyle.GetStyleProperty<bool>("gtaui.menus.multiSelectionMenu.defaults.showBackButton");
            HideDisabledOptions = uiStyle.GetStyleProperty<bool>("gtaui.menus.multiSelectionMenu.defaults.hideDisabledOptions");
            ShowSelectAllButton = uiStyle.GetStyleProperty<bool>("gtaui.menus.multiSelectionMenu.defaults.showSelectAllButton");
            ShowDeselectAllButton = uiStyle.GetStyleProperty<bool>("gtaui.menus.multiSelectionMenu.defaults.showDeselectAllButton");
            MenuInstance.Closed += MenuClosed;
        }

        private void MenuClosed(object sender, EventArgs e)
        {
            if (itemsHaveBeenSelected == false)
            {
                onCancel?.Invoke();
                itemsHaveBeenSelected = false;
            }
        }

        /// <summary>
        /// Create a menu item for the given option.
        /// The title and description do not need to be set.
        /// If you want to use the default behaviour when activating the menu item, pass the <see cref="OptionMenuItemACheckboxChanged"/> method as the onActivated action.
        /// </summary>
        /// <param name="option">The option to create a menu item for.</param>
        /// <returns>A menu item for the given option.</returns>
        protected virtual CheckBoxMenuItem CreateMenuItemForOption(T option)
        {

            return new CheckBoxMenuItem(string.Empty, string.Empty, OptionMenuItemACheckboxChanged);
        }

        /// <summary>
        /// Create a menu item that acts as a close button.
        /// Activating this menu item should call <see cref="Menu.Close"/>.
        /// </summary>
        /// <returns>A menu item that acts as a close button.</returns>
        protected virtual MenuItem CreateCloseButton()
        {
            CloseMenuItem item = new CloseMenuItem();
            item.Title = uiStyle.GetStyleProperty<string>("gtaui.menus.multiSelectionMenu.closeMenuItem.title");
            item.Description = uiStyle.GetStyleProperty<string>("gtaui.menus.multiSelectionMenu.closeMenuItem.description");

            return item;
        }

        /// <summary>
        /// Create a menu item that acts as a back button.
        /// Activating this menu item should call <see cref="Menu.Back"/>.
        /// </summary>
        /// <returns>A menu item that acts as a back button.</returns>
        protected virtual MenuItem CreateBackButton()
        {
            BackMenuItem item = new BackMenuItem();
            item.Title = uiStyle.GetStyleProperty<string>("gtaui.menus.multiSelectionMenu.backMenuItem.title");
            item.Description = uiStyle.GetStyleProperty<string>("gtaui.menus.multiSelectionMenu.backMenuItem.description");

            return item;
        }

        /// <summary>
        /// Create a menu item that selects all options.
        /// Use the <see cref="SelectAllOptions(MenuItem)"/> method for the action that should trigger the selection of all options.
        /// </summary>
        /// <returns>A menu item that will select all options when triggered correctly</returns>
        protected virtual MenuItem CreateSelectAllButton()
        {
            string title = uiStyle.GetStyleProperty<string>("gtaui.menus.multiSelectionMenu.selectAllMenuItem.title");
            string description = uiStyle.GetStyleProperty<string>("gtaui.menus.multiSelectionMenu.selectAllMenuItem.description");
            ButtonMenuItem item = new ButtonMenuItem(title, description, SelectAllOptions);

            return item;
        }

        /// <summary>
        /// Create a menu item that deselects all options.
        /// Use the <see cref="DeselectAllOptions(MenuItem)"/> method for the action that should trigger the deselection of all options.
        /// </summary>
        /// <returns>A menu item that will deselect all options when triggered correctly</returns>
        protected virtual MenuItem CreateDeselectAllButton()
        {
            string title = uiStyle.GetStyleProperty<string>("gtaui.menus.multiSelectionMenu.deselectAllMenuItem.title");
            string description = uiStyle.GetStyleProperty<string>("gtaui.menus.multiSelectionMenu.deselectAllMenuItem.description");
            ButtonMenuItem item = new ButtonMenuItem(title, description, SelectAllOptions);

            return item;
        }

        /// <summary>
        /// Create a menu item that accepts the current selection.
        /// Use the <see cref="AcceptSelection(MenuItem)"/> method for the action that should accept the current selection.
        /// </summary>
        /// <returns>A menu item that will accept the current selection.</returns>
        protected virtual MenuItem CreateAcceptSelectionMenuItem()
        {
            string title = uiStyle.GetStyleProperty<string>("gtaui.menus.multiSelectionMenu.acceptSelectionMenuItem.title");
            string description = uiStyle.GetStyleProperty<string>("gtaui.menus.multiSelectionMenu.acceptSelectionMenuItem.description");
            ButtonMenuItem item = new ButtonMenuItem(title, description, AcceptSelection);

            return item;
        }

        /// <summary>
        /// Default handler method for when an option menu item is activated.
        /// </summary>
        /// <param name="item">The activated menu item.</param>
        protected void OptionMenuItemACheckboxChanged(MenuItem item, bool isSelected)
        {
            if (item.Item.Tag == null || item.Item.Tag is T == false)
            {
                UIController.Log($"Error: option menu item '{item.Title}' was selected but it's Tag property does not contain valid data!");
                return;
            }

            T option = (T)item.Item.Tag;

            if (isSelected)
            {
                selectedOptions.Add(option);
                itemsHaveBeenSelected = true;
                acceptMenuItem.Item.Enabled = true;
            }
            else
            {
                selectedOptions.Remove(option);
                if (selectedOptions.Count == 0)
                {
                    itemsHaveBeenSelected = false;
                    acceptMenuItem.Item.Enabled = false;
                }
            }

            if (massUpdateHappening == false)
            {
                OnSelectionChanged?.Invoke(selectedOptions);
            }
        }

        /// <summary>
        /// Pass this function to an action that needs to select all options.
        /// </summary>
        /// <param name="_">Unused.</param>
        protected void SelectAllOptions(MenuItem _)
        {
            UpdateAllOptions(true);
        }

        /// <summary>
        /// Pass this function to an action that needs to deselect all options.
        /// </summary>
        /// <param name="_">Unused.</param>
        protected void DeselectAllOptions(MenuItem _)
        {
            UpdateAllOptions(false);
        }

        private void UpdateAllOptions(bool newState)
        {
            massUpdateHappening = true;
            foreach (MenuItem item in Items)
            {
                if (item is CheckBoxMenuItem == false)
                {
                    continue;
                }

                LemonUI.Menus.NativeCheckboxItem checkBoxItem = item.Item as LemonUI.Menus.NativeCheckboxItem;
                checkBoxItem.Checked = newState;
            }

            OnSelectionChanged?.Invoke(selectedOptions);

            massUpdateHappening = false;
        }

        /// <summary>
        /// Pass this method to an action that needs to accept the current selection.
        /// </summary>
        /// <param name="_">Unused.</param>
        protected void AcceptSelection(MenuItem _)
        {
            onSelection?.Invoke(selectedOptions);
        }

        /// <summary>
        /// Returns whether the given option should be enabled or not.
        /// Note: this method will only be called when <see cref="OptionShouldBeEnabledFunc"/> has it's default value.
        /// </summary>
        /// <param name="option">The option to check.</param>
        /// <returns><c>true</c> if the option should be enabled. <c>false</c> otherwise.</returns>
        protected virtual bool OptionShouldBeEnabled(T option)
        {
            return true;
        }

        /// <summary>
        /// Build the multi selection menu before showing it.
        /// </summary>
        /// <returns><c>true</c> if the menu is valid and will be shown. <c>false</c> otherwise.</returns>
        protected override bool BeforeShow()
        {
            if (options.Any() == false)
            {
                UIController.Log($"Warning: Unable to show multi selection menu with title '{Title}' because it has no items.");
                return false;
            }

            ClearMenuItems();
            selectedOptions.Clear();

            acceptMenuItem = CreateAcceptSelectionMenuItem();

            acceptMenuItem.Item.Enabled = false;

            AddMenuItem(acceptMenuItem);

            if (ShowSelectAllButton)
            {
                AddMenuItem(CreateSelectAllButton());
            }

            if (ShowDeselectAllButton)
            {
                AddMenuItem(CreateDeselectAllButton());
            }

            foreach (T item in options)
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
                    enabled = (item is IMenuSelectable) == false || (item as IMenuSelectable).IsMenuItemEnabled();
                }


                if (HideDisabledOptions && enabled == false)
                {
                    continue;
                }

                CheckBoxMenuItem menuItem = CreateMenuItemForOption(item);
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
    }
}

