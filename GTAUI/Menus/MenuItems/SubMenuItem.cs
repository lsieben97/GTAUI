using LemonUI.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.Menus.MenuItems
{
    /// <summary>
    /// A menu item that looks like a <see cref="ButtonMenuItem"/> and shows another <see cref="Menu"/> when activated.
    /// </summary>
    public class SubMenuItem : MenuItem
    {
        private Menu subMenu;

        /// <summary>
        /// The name of the method to call to get the <see cref="Menu"/> to show.
        /// </summary>
        public string GetSubmenuFunc { get; set; }

        /// <summary>
        /// The name of the <see cref="Menu"/> to show. Must be registered through <see cref="UIController.RegisterAssemblyMenus(Assembly)"/>.
        /// </summary>
        public string SubmenuName { get; set; }

        /// <summary>
        /// Empty constructor for json serialization.
        /// For internal use only.
        /// </summary>
        public SubMenuItem() { }

        /// <summary>
        /// Create a new sub menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="subMenu">The menu to show when this menu item is activated.</param>
        public SubMenuItem(string id, string title, Menu subMenu) : this(id, title, string.Empty, subMenu, null) { }

        /// <summary>
        /// Create a new sub menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="subMenu">The menu to show when this menu item is activated.</param>
        public SubMenuItem(string id, string title, string description, Menu subMenu) : this(id, title, description, subMenu, null) { }

        /// <summary>
        /// Create a new sub menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="subMenu">The menu to show when this menu item is activated.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        public SubMenuItem(string id, string title, Menu subMenu, Action<MenuItem> onSelected) : this(id, title, string.Empty, subMenu, onSelected) { }

        /// <summary>
        /// Create a new sub menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="subMenu">The menu to show when this menu item is activated.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SubMenuItem(string id, string title, string description, Menu subMenu, Action<MenuItem> onSelected)
        {
            if (title is null)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
            Title = title;
            SelectedItemMethod = new InvokableMethod<MenuItem>(onSelected);
            this.subMenu = subMenu;

            Item = new NativeItem(Title, description ?? string.Empty);

            Item.Activated += SubMenuItemActivated;
            Item.Selected += ItemSelected;
            Item.EnabledChanged += ItemEnabledChanged;
        }

        
        private void SubMenuItemActivated(object sender, EventArgs e)
        {
            ParentMenu.MenuInstance.Visible = false;
            subMenu.MenuInstance.Parent = ParentMenu.MenuInstance;
            subMenu.Show();
        }

        private Menu GetSubMenu(object eventTarget, Type eventTargetType)
        {
            MethodInfo getMenuMethod = ReflectionHelper.GetMethodWithReturnType(GetSubmenuFunc, typeof(Menu), eventTargetType);
            if (getMenuMethod != null)
            {
                return (Menu)getMenuMethod.Invoke(eventTarget, new object[] { });
            }

            if (SubmenuName == null)
            {
                return null;
            }

            return UIController.GetInstance().GetMenuInstance(SubmenuName);
        }

        /// <inheritdoc/>
        public override void InitializeFromExistingProperties(object eventTarget, Type eventTargetType)
        {
            Item = new NativeItem(Title, Description ?? string.Empty);
            EventTarget = eventTarget;
            EventTargetType = eventTargetType;

            ParentMenu = (eventTarget as Menu);

            subMenu = GetSubMenu(eventTarget, eventTargetType);

            if (subMenu == null)
            {
                UIController.Log($"Unable to get sub menu for menu item with title {Title}. Either the GetSubmenuFunc property or the SubmenuName property needs to be non null.");
                IsValid = false;
                return;
            }

            SelectedItemMethod = new InvokableMethod<MenuItem>(ReflectionHelper.GetMethodWithArguments(OnSelected, new Type[] { typeof(MenuItem) }, EventTargetType));
            EnabledChangedMethod = new InvokableMethod<MenuItem>(ReflectionHelper.GetMethodWithArguments(OnEnabledChanged, new Type[] { typeof(MenuItem) }, EventTargetType));

            Item.Activated += SubMenuItemActivated;
            Item.Selected += ItemSelected;
            Item.EnabledChanged += ItemEnabledChanged;

            SetBadges();
        }
    }
}
