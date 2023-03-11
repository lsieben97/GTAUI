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
    /// A menu item that represent a list of numerical values the user can scroll through.
    /// Wraps the <see cref="NativeSliderItem"/> class.
    /// </summary>
    public class SliderMenuItem :MenuItem
    {
        private InvokableMethod<MenuItem, int> itemSelectedMethod;

        /// <summary>
        /// The maximum value the user can choose.
        /// </summary>
        public int Maximum { get; set; }

        /// <summary>
        /// The name of the method that returns the maximum value the user can choose.
        /// </summary>
        public string GetMaximumFunc { get; set; }

        /// <summary>
        /// The name of the method to call when the selected value changes.
        /// </summary>
        public string OnItemSelected { get; set; }

        /// <summary>
        /// Empty constructor for json serialization.
        /// For internal use only.
        /// </summary>
        public SliderMenuItem() { }

        /// <summary>
        /// Create a new slider menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="maximum">The maximum value the user can choose.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        public SliderMenuItem(string id, string title, int maximum, Action<MenuItem, int> onSelectionChanged) : this(id, title, string.Empty, maximum, onSelectionChanged) { }

        /// <summary>
        /// Create a new slider menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="maximum">The maximum value the user can choose.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        public SliderMenuItem(string id, string title, string description, int maximum, Action<MenuItem, int> onSelectionChanged) : this(id, title, description, maximum, onSelectionChanged, null) { }

        /// <summary>
        /// Create a new slider menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="maximum">The maximum value the user can choose.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        public SliderMenuItem(string id, string title, int maximum, Action<MenuItem, int> onSelectionChanged, Action<MenuItem> onSelected) : this(id, title, string.Empty, maximum, onSelectionChanged, onSelected) { }

        /// <summary>
        /// Create a new slider menu item.
        /// </summary>
        /// <param name="id">The id of this menu item.</param>
        /// <param name="title">The title of this menu item.</param>
        /// <param name="description">The description of this menu item.</param>
        /// <param name="maximum">The maximum value the user can choose.</param>
        /// <param name="onSelectionChanged">Action to invoke when the selected value changes.</param>
        /// <param name="onSelected">The action to invoke when this menu item is selected.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SliderMenuItem(string id, string title, string description, int maximum, Action<MenuItem, int> onSelectionChanged, Action<MenuItem> onSelected)
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
            itemSelectedMethod = new InvokableMethod<MenuItem, int>(onSelectionChanged);

            NativeSliderItem nativeItem = new NativeSliderItem(title, description ?? string.Empty);

            nativeItem.Maximum = maximum;
            nativeItem.ValueChanged += ItemValueChanged;
            Item = nativeItem;
            AttachEventHandlers();
        }

        private void ItemValueChanged(object sender, EventArgs e)
        {
            if (itemSelectedMethod != null)
            {
                itemSelectedMethod.Invoke(EventTarget, this, (Item as NativeSliderItem).Value);
            }
        }

        private int GetMaximum(object eventTarget)
        {
            MethodInfo getMaximumMethod = ReflectionHelper.GetMehodWithReturnType(GetMaximumFunc, typeof(int), EventTargetType);
            if (getMaximumMethod == null)
            {
                return Maximum;
            }

            return (int)getMaximumMethod.Invoke(eventTarget, new object[] { });
        }

        /// <inheritdoc/>
        public override void InitializeFromExistingProperties(object eventTarget, Type eventTargetType)
        {
            NativeSliderItem nativeItem = new NativeSliderItem(Title, Description ?? string.Empty);
            EventTarget = eventTarget;
            EventTargetType = eventTargetType;

            ParentMenu = (eventTarget as Menu).MenuInstance;

            nativeItem.Maximum = GetMaximum(eventTarget);

            itemSelectedMethod = new InvokableMethod<MenuItem, int>(ReflectionHelper.GetMethodWithArguments(OnItemSelected, new Type[] { typeof(MenuItem), typeof(int) }, EventTargetType));

            nativeItem.ValueChanged += ItemValueChanged;
            Item = nativeItem;

            SetBadges();
        }
    }
}
