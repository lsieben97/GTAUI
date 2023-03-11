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
    /// A menu item that represents a checkbox the user can check or uncheck.
    /// </summary>
    public class CheckBoxMenuItem : MenuItem
    {
        private InvokableMethod<MenuItem, bool> checkboxChangedMethod;

        /// <summary>
        /// The name of the method that will be called when the state of the checkbox changes.
        /// </summary>
        public string OnCheckboxChanged { get; set; }


        /// <summary>
        /// Empty constructor for json serialization.
        /// For internal use only.
        /// </summary>
        public CheckBoxMenuItem() { }

        /// <summary>
        /// Create a new checkbox menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="onCheckboxChanged">The action to invoke when the state of the checkbox changes.</param>
        public CheckBoxMenuItem(string id, string title, Action<MenuItem, bool> onCheckboxChanged) : this(id, title, string.Empty, onCheckboxChanged) { }

        /// <summary>
        /// Create a new checkbox menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="onCheckboxChanged">The action to invoke when the state of the checkbox changes.</param>
        public CheckBoxMenuItem(string id, string title, string description, Action<MenuItem, bool> onCheckboxChanged) : this(id, title, description, onCheckboxChanged, null) { }

        /// <summary>
        /// Create a new checkbox menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="onCheckboxChanged">The action to invoke when the state of the checkbox changes.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        public CheckBoxMenuItem(string id, string title, Action<MenuItem, bool> onCheckboxChanged, Action<MenuItem> onSelected) : this(id, title, string.Empty, onCheckboxChanged, onSelected) { }

        /// <summary>
        /// Create a new checkbox menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="onCheckboxChanged">The action to invoke when the state of the checkbox changes.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CheckBoxMenuItem(string id, string title, string description, Action<MenuItem, bool> onCheckboxChanged, Action<MenuItem> onSelected)
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
            checkboxChangedMethod = new InvokableMethod<MenuItem, bool>(onCheckboxChanged);

            NativeCheckboxItem nativeItem = new NativeCheckboxItem(title, description ?? string.Empty);
            nativeItem.CheckboxChanged += ItemCheckboxChanged;
            Item = nativeItem;
            AttachEventHandlers();
        }

        private void ItemCheckboxChanged(object sender, EventArgs e)
        {
            checkboxChangedMethod?.Invoke(EventTarget, this, (Item as NativeCheckboxItem).Checked);
        }

        /// <inheritdoc/>
        public override void InitializeFromExistingProperties(object eventTarget, Type eventTargetType)
        {
            NativeCheckboxItem nativeItem = new NativeCheckboxItem(Title, Description ?? string.Empty);
            EventTarget = eventTarget;
            EventTargetType = eventTargetType;

            ParentMenu = (eventTarget as Menu).MenuInstance;

            checkboxChangedMethod = new InvokableMethod<MenuItem, bool>(ReflectionHelper.GetMethodWithArguments(OnCheckboxChanged, new Type[] { typeof(MenuItem), typeof(bool) }, EventTargetType));
            nativeItem.CheckboxChanged += ItemCheckboxChanged;
            Item = nativeItem;

            SetBadges();
        }
    }
}
