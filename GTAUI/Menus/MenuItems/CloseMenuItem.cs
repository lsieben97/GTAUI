using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTAUI.Styling;

namespace GTAUI.Menus.MenuItems
{
    /// <summary>
    /// A menu item that calls the <see cref="Menu.Close"/> method when activated.
    /// The title and description can be changed through <see cref="Styling.UIStyle"/> with the
    /// <c>gtaui.menus.closeMenuItem.title</c> and <c>gtaui.menus.closeMenuItem.description</c> style properties.
    /// </summary>
    public class CloseMenuItem : MenuItem
    {
        /// <summary>
        /// Create a new close menu item.
        /// </summary>
        public CloseMenuItem() : this(null) { }

        /// <summary>
        /// Create a new close menu item.
        /// </summary>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        public CloseMenuItem(Action<MenuItem> onSelected)
        {
            UIStyle uiStyle = UIStyle.GetInstance();

            string title = uiStyle.GetStyleProperty<string>("gtaui.menus.closeMenuItem.title");
            string description = uiStyle.GetStyleProperty<string>("gtaui.menus.closeMenuItem.description");

            Item = new LemonUI.Menus.NativeItem(title, description);
            Title = Item.Title;

            SelectedItemMethod = new InvokableMethod<MenuItem>(onSelected);

            Item.Activated += CloseItemActivated;
            Item.Selected += ItemSelected;
            Item.EnabledChanged += ItemEnabledChanged;

        }

        private void CloseItemActivated(object sender, EventArgs e)
        {
            ParentMenu.Visible = false;
        }

        /// <inheritdoc/>
        public override void InitializeFromExistingProperties(object eventTarget, Type eventTargetType)
        {
            EventTarget = eventTarget;
            EventTargetType = eventTargetType;

            ParentMenu = (eventTarget as Menu).MenuInstance;
        }
    }
}
