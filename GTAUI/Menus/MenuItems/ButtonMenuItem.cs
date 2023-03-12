using System;

namespace GTAUI.Menus.MenuItems
{
    /// <summary>
    /// A menu item that represents normal item.
    /// </summary>
    public class ButtonMenuItem : MenuItem
    {
        /// <summary>
        /// Empty constructor for json serialization.
        /// For internal use only.
        /// </summary>
        public ButtonMenuItem() { }

        /// <summary>
        /// Create a new button menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="onActivated">The action to invoke when the menu item is activated.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ButtonMenuItem(string id, string title, Action<MenuItem> onActivated) : this(id, title, string.Empty, onActivated) { }

        /// <summary>
        /// Create a new button menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="onActivated">The action to invoke when the menu item is activated.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ButtonMenuItem(string id, string title, string description, Action<MenuItem> onActivated) : this(id, title, description, onActivated, null) { }

        /// <summary>
        /// Create a new button menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="onActivated">The action to invoke when this menu item is activated.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ButtonMenuItem(string id, string title, Action<MenuItem> onActivated, Action<MenuItem> onSelected) : this(id, title, string.Empty, onActivated, onSelected) { }

        /// <summary>
        /// Create a new button menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="onActivated">The action to invoke when this menu item is activated.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ButtonMenuItem(string id, string title, string description, Action<MenuItem> onSelected, Action<MenuItem> onActivated)
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
            ActivatedMethod = new InvokableMethod<MenuItem>(onActivated);

            Item = new LemonUI.Menus.NativeItem(Title, description ?? string.Empty);
            AttachEventHandlers();
        }

        /// <inheritdoc/>
        public override void InitializeFromExistingProperties(object eventTarget, Type eventTargetType)
        {
            EventTarget = eventTarget;
            EventTargetType = eventTargetType;
            Item = new LemonUI.Menus.NativeItem(Title, Description ?? string.Empty);
            ValidateAndAttachEventFunctions();
            SetBadges();
            ParentMenu = (eventTarget as Menu);
        }
    }
}
