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
    /// A menu item that represents a list of values the user can scroll through.
    /// The <see cref="object.ToString"/> method will be called on each object to get a string representation.
    /// </summary>
    public class ListMenuItem : MenuItem
    {
        private InvokableMethod<MenuItem, object> itemSelectedMethod;

        /// <summary>
        /// The name of the method to call to get the list of values to show.
        /// </summary>
        public string GetListFunc { get; set; }

        /// <summary>
        /// The name of the method to call when the selected value changes.
        /// </summary>
        public string OnItemSelected { get; set; }

        /// <summary>
        /// Empty constructor for json serialization.
        /// For internal use only.
        /// </summary>
        public ListMenuItem() { }

        /// <summary>
        /// Create a new list menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="list">The list of values to show.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        public ListMenuItem(string id, string title, IEnumerable<object> list, Action<MenuItem, object> onSelectionChanged) : this(id, title, string.Empty, list, onSelectionChanged) { }

        /// <summary>
        /// Create a new list menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description"></param>
        /// <param name="list">The list of values to show.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        public ListMenuItem(string id, string title, string description, IEnumerable<object> list, Action<MenuItem, object> onSelectionChanged) : this(id, title, description, list, onSelectionChanged, null) { }

        /// <summary>
        /// Create a new list menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="list">The list of values to show.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        public ListMenuItem(string id, string title, IEnumerable<object> list, Action<MenuItem, object> onSelectionChanged, Action<MenuItem> onSelected) : this(id, title, string.Empty, list, onSelectionChanged, onSelected) { }

        /// <summary>
        /// Create a new list menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description"></param>
        /// <param name="list">The list of values to show.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ListMenuItem(string id, string title, string description, IEnumerable<object> list, Action<MenuItem, object> onSelectionChanged, Action<MenuItem> onSelected)
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
            itemSelectedMethod = new InvokableMethod<MenuItem, object>(onSelectionChanged);

            NativeListItem<object> nativeItem = new NativeListItem<object>(title, description ?? string.Empty);

            foreach(object item in list)
            {
                nativeItem.Add(item);
            }

            nativeItem.ItemChanged += ItemItemChanged;
            Item = nativeItem;
            AttachEventHandlers();
        }

        private void ItemItemChanged(object sender, ItemChangedEventArgs<object> e)
        {
            itemSelectedMethod?.Invoke(EventTarget, this, e.Object);
        }

        /// <inheritdoc/>
        public override void InitializeFromExistingProperties(object eventTarget, Type eventTargetType)
        {
            NativeListItem<object> nativeItem = new NativeListItem<object>(Title, Description ?? string.Empty);
            EventTarget = eventTarget;
            EventTargetType = eventTargetType;

            ParentMenu = (eventTarget as Menu);

            MethodInfo getListMethod = ReflectionHelper.GetMethodWithReturnType(GetListFunc, typeof(IEnumerable<object>), EventTargetType);
            if (getListMethod == null)
            {
                UIController.Log($"Warning: menu item with title {Title} is a list menu item but it's GetList function could not be found. This menu item will not be displayed.");
                IsValid = false;
                return;
            }


            if ((getListMethod.Invoke(eventTarget, new object[] { }) is IEnumerable<object> listItems) == false)
            {
                UIController.Log($"Warning: menu item with title {Title} is a list menu item but it's GetList function returned null. This menu item will not be displayed.");
                IsValid = false;
                return;
            }

            foreach (object obj in listItems)
            {
                nativeItem.Add(obj);
            }

            itemSelectedMethod = new InvokableMethod<MenuItem, object>(ReflectionHelper.GetMethodWithArguments(OnItemSelected, new Type[] { typeof(MenuItem), typeof(object) }, EventTargetType));

            nativeItem.ItemChanged += ItemItemChanged;
            Item = nativeItem;

            SetBadges();
        }
    }
}
