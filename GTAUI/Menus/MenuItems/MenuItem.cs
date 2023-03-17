using GTAUI.Json;
using LemonUI.Elements;
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
    /// Base class for all menu items.
    /// Provides default implemetations for the <see cref="NativeItem.Activated"/>, <see cref="NativeItem.Selected"/> and <see cref="NativeItem.EnabledChanged"/> events.
    /// </summary>
    public abstract class MenuItem
    {
        private string title = "Title";
        private string description = string.Empty;

        /// <summary>
        /// The badge (texture) displayed on the right side of the menu item.
        /// </summary>
        public MenuItemBadge RightBadge { get; protected set; } = null;

        /// <summary>
        /// The badge (texture) displayed on the left side of the menu item.
        /// </summary>
        public MenuItemBadge LeftBadge { get; protected set; } = null;

        /// <summary>
        /// The id of the menu item.
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// The title of the menu item.
        /// </summary>
        public string Title { get => title; set => SetTitle(value); }

        /// <summary>
        /// The description of the menu item.
        /// </summary>
        public string Description { get => description; set => SetDescription(value); }
        /// <summary>
        /// The name of the method to call when the menu item is selected.
        /// </summary>
        public string OnSelected { get; set; }

        /// <summary>
        /// The name of the method to call when the menu item is activated.
        /// </summary>
        public string OnActivated { get; set; }

        /// <summary>
        /// The name of the method to call when the <see cref="NativeItem.Enabled"/> property is changed.
        /// </summary>
        public string OnEnabledChanged { get; set; }

        /// <summary>
        /// The Actual item being displayed.
        /// </summary>
        public NativeItem Item { get; protected set; }

        /// <summary>
        /// The <see cref="Menu"/> that this menu item is part of.
        /// </summary>
        public Menu ParentMenu { get; internal set; }

        /// <summary>
        /// <c>true</c> when this item is property configured. <c>false</c> otherwise.
        /// This item will not be included in the <see cref="ParentMenu"/> when this property is <c>false</c>.
        /// Set this property to <c>false</c> when method names of event handlers are incorrect or invalid.
        /// </summary>
        public bool IsValid { get; protected set; } = true;

        /// <summary>
        /// The type of the menu item.
        /// </summary>
        [JsonTypeMap("menuItems", "Button")]
        public string Type { get; set; } = "Button";

        /// <summary>
        /// The method or action that will be invoked when the menu item is selected.
        /// </summary>
        protected InvokableMethod<MenuItem> SelectedItemMethod { get; set; } = null;

        /// <summary>
        /// The method or action that will be invoked when the <see cref="NativeItem.Enabled"/> property changes.
        /// </summary>
        protected InvokableMethod<MenuItem> EnabledChangedMethod { get; set; } = null;

        /// <summary>
        /// The method or action that will be invoked when the menu item is activated.
        /// </summary>
        protected InvokableMethod<MenuItem> ActivatedMethod { get; set; } = null;

        /// <summary>
        /// The object that various methods will be invoked on.
        /// </summary>
        protected object EventTarget { get; set; } = null;

        /// <summary>
        /// The type of the <see cref="EventTarget"/> object.
        /// </summary>
        protected Type EventTargetType { get; set; }

        /// <summary>
        /// The name of the method the will be invoked when the menu item is selected.
        /// </summary>
        protected string OnSelectedFunctionName { get; set; }

        /// <summary>
        /// The name of the method that will be invoked when the menu item is activated.
        /// </summary>
        protected string OnActivatedFunctionName { get; set; }

        /// <summary>
        /// The name of the method that will be invoked when the <see cref="NativeItem.Enabled"/> property property changes.
        /// </summary>
        protected string OnEnabledChangedFunctionName { get; set; }


        /// <summary>
        /// Initialize this menu item based on the filled properties.
        /// This should connect the correct event handlers based on properties like <see cref="OnActivated"/>.
        /// </summary>
        /// <param name="eventTarget">The object that has methods referenced by properties like <see cref="OnActivated"/>.</param>
        /// <param name="eventTargetType">The type of the <paramref name="eventTarget"/> object.</param>
        public abstract void InitializeFromExistingProperties(object eventTarget, Type eventTargetType);

        /// <summary>
        /// Set the texture to display on the left of the menu item.
        /// </summary>
        /// <param name="dictionary">The name of the dictionary where the <paramref name="texture"/> can be found.</param>
        /// <param name="texture">The name of the texture to show.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetLeftBadge(string dictionary, string texture)
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (texture is null)
            {
                throw new ArgumentNullException(nameof(texture));
            }

            Item.LeftBadge = new ScaledTexture(dictionary, texture);
        }

        /// <summary>
        /// Set the texture to display on the right of the menu item.
        /// </summary>
        /// <param name="dictionary">The name of the dictionary where the <paramref name="texture"/> can be found.</param>
        /// <param name="texture">The name of the texture to show.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetRightBadge(string dictionary, string texture)
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (texture is null)
            {
                throw new ArgumentNullException(nameof(texture));
            }

            Item.RightBadge = new ScaledTexture(dictionary, texture);
        }

        /// <summary>
        /// Set the action to invoke when the menu item is activated.
        /// </summary>
        /// <param name="onActivated">The action to invoke when the menu item is activated.</param>
        public void SetOnActivated(Action<MenuItem> onActivated)
        {
            ActivatedMethod = new InvokableMethod<MenuItem>(onActivated.Method);
        }

        /// <summary>
        /// Set the action to invoke when the menu item is selected.
        /// </summary>
        /// <param name="onActivated">The action to invoke when the menu item is selected.</param>
        public void SetOnSelected(Action<MenuItem> onSelected)
        {
            SelectedItemMethod = new InvokableMethod<MenuItem>(onSelected.Method);
        }

        /// <summary>
        /// Set the action to invoke when the <see cref="NativeItem.Enabled"/> property is changed.
        /// </summary>
        /// <param name="onActivated">The action to invoke when the <see cref="NativeItem.Enabled"/> property is changed.</param>
        public void SetOnItemEnabledChanged(Action<MenuItem> onEnabledChanged)
        {
            EnabledChangedMethod = new InvokableMethod<MenuItem>(onEnabledChanged.Method);
        }

        /// <summary>
        /// Resolve and attach the event handlers for the <see cref="OnActivated"/>, <see cref="OnSelected"/> and <see cref="OnEnabledChanged"/> events.
        /// Calls <see cref="AttachEventHandlers"/>.
        /// </summary>
        protected void ValidateAndAttachEventFunctions()
        {
            SelectedItemMethod = new InvokableMethod<MenuItem>(ReflectionHelper.GetMethodWithArguments(OnSelected, new Type[] { typeof(MenuItem) }, EventTargetType));
            EnabledChangedMethod = new InvokableMethod<MenuItem>(ReflectionHelper.GetMethodWithArguments(OnEnabledChanged, new Type[] { typeof(MenuItem) }, EventTargetType));
            ActivatedMethod = new InvokableMethod<MenuItem>(ReflectionHelper.GetMethodWithArguments(OnActivated, new Type[] { typeof(MenuItem) }, EventTargetType));

            AttachEventHandlers();
        }

        /// <summary>
        /// Attaches the default event handlers fot the <see cref="NativeItem.Activated"/>, <see cref="NativeItem.Selected"/> and <see cref="NativeItem.EnabledChanged"/> events.
        /// The default event handlers invoke the method or action that was resolved or set. This is done for the method connected via <see cref="OnActivated"/>, <see cref="OnSelected"/> and <see cref="OnEnabledChanged"/> properties.
        /// </summary>
        protected void AttachEventHandlers()
        {

            Item.Activated += ItemActivated;
            Item.Selected += ItemSelected;
            Item.EnabledChanged += ItemEnabledChanged;
        }

        /// <summary>
        /// Set left and right badges on the <see cref="Item"/> using the <see cref="LeftBadge"/> and <see cref="RightBadge"/> properties.
        /// </summary>
        protected void SetBadges()
        {
            if (LeftBadge != null)
            {
                Item.LeftBadge = new ScaledTexture(LeftBadge.Dictionary, LeftBadge.Texture);
            }

            if (RightBadge != null)
            {
                Item.RightBadge = new ScaledTexture(RightBadge.Dictionary, RightBadge.Texture);
            }
        }

        /// <summary>
        /// Default event handler for the <see cref="NativeItem.Activated"/> event.
        /// Invokes the resolved method from the <see cref="OnActivated"/> property if available.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ItemEnabledChanged(object sender, EventArgs e)
        {
            if (EnabledChangedMethod != null)
            {
                EnabledChangedMethod.Invoke(EventTarget, this);
            }
        }

        /// <summary>
        /// Default event handler for the <see cref="NativeItem.Selected"/> event.
        /// Invokes the resolved method from the <see cref="OnSelected"/> property if available.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ItemSelected(object sender, SelectedEventArgs e)
        {
            if (SelectedItemMethod != null)
            {
                SelectedItemMethod.Invoke(EventTarget, this);
            }
        }

        /// <summary>
        /// Default event handler for the <see cref="NativeItem.EnabledChanged"/> event.
        /// Invokes the resolved method from the <see cref="OnEnabledChanged"/> property if available.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ItemActivated(object sender, EventArgs e)
        {
            if (ActivatedMethod != null)
            {
                ActivatedMethod.Invoke(EventTarget, this);
            }
        }


        private void SetTitle(string title)
        {
            this.title = title;
            if (Item == null)
            {
                return;
            }

            Item.Title = title;
        }

        private void SetDescription(string description)
        {
            this.description = description;
            if (Item == null)
            {
                return;
            }

            Item.Description = description;
        }
    }
}
