using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTAUI.Styling;

namespace GTAUI.Menus.MenuItems
{
    /// <summary>
    /// A menu item that calls the <see cref="Menu.Back"/> method when activated.
    /// The title and description can be changed through <see cref="Styling.UIStyle"/> with the
    /// <c>gtaui.menus.backMenuItem.title</c> and <c>gtaui.menus.backMenuItem.description</c> style properties.
    /// </summary>
    public class BackMenuItem : MenuItem
    {
        /// <summary>
        /// Create a new back menu item.
        /// </summary>
        public BackMenuItem() : this(null) { }

        /// <summary>
        /// Create a new back menu item wich calls the given action when selected.
        /// </summary>
        /// <param name="onSelected">The action to call when the item gets selected.</param>
        public BackMenuItem(Action<MenuItem> onSelected)
        {
            UIStyle uiStyle = UIStyle.GetInstance();

            string title = uiStyle.GetStyleProperty<string>("gtaui.menus.backMenuItem.title");
            string description = uiStyle.GetStyleProperty<string>("gtaui.menus.backMenuItem.description");

            Item = new LemonUI.Menus.NativeItem(title, description);
            Title = Item.Title;
            Description = Item.Description;
            SelectedItemMethod = new InvokableMethod<MenuItem>(onSelected);

            Item.Activated += BackItemActivated;
            Item.Selected += ItemSelected;
            Item.EnabledChanged += ItemEnabledChanged;

        }

        private void BackItemActivated(object sender, EventArgs e)
        {
            ParentMenu.Back();
        }

        /// <inheritdoc/>
        public override void InitializeFromExistingProperties(object eventTarget, Type eventTargetType)
        {
            EventTarget = eventTarget;
            EventTargetType = eventTargetType;

            SelectedItemMethod = new InvokableMethod<MenuItem>(ReflectionHelper.GetMethodWithArguments(OnSelected, new Type[] { typeof(MenuItem) }, EventTargetType));
            EnabledChangedMethod = new InvokableMethod<MenuItem>(ReflectionHelper.GetMethodWithArguments(OnEnabledChanged, new Type[] { typeof(MenuItem) }, EventTargetType));

            ParentMenu = (eventTarget as Menu).MenuInstance;
        }
    }
}
