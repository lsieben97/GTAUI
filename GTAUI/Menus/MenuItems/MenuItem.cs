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
    public abstract class MenuItem
    {
        public MenuItemBadge RightBadge { get; set; } = null;

        public MenuItemBadge LeftBadge { get; set; } = null;
        public string Id { get; set; } = "";
        public string Title { get; set; } = "Title";
        public string Description { get; set; } = null;
        public string OnSelected { get; set; }
        public string OnActivated { get; set; }
        public string OnEnabledChanged { get; set; }

        [JsonTypeMap("menuItems", "Button")]
        public string Type { get; set; } = "Button";

        protected InvokableMethod<MenuItem> SelectedItemMethod { get; set; } = null;
        protected InvokableMethod<MenuItem> EnabledChangedMethod { get; set; } = null;
        protected InvokableMethod<MenuItem> ActivatedMethod { get; set; } = null;
        protected object EventTarget { get; set; } = null;
        protected Type EventTargetType { get; set; }
        protected string OnSelectedFunctionName { get; set; }
        protected string OnActivatedFunctionName { get; set; }
        protected string OnEnabledChangedFunctionName { get; set; }

        public NativeItem Item { get; protected set; }
        public Menu ParentMenu { get; set; }
        public bool IsValid { get; protected set; } = true;

        public abstract void InitializeFromExistingProperties(object eventTarget, Type eventTargetType);

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

        public void SetOnActivated(Action<MenuItem> onActivated)
        {
            ActivatedMethod = new InvokableMethod<MenuItem>(onActivated.Method);
        }

        public void SetOnSelected(Action<MenuItem> onSelected)
        {
            SelectedItemMethod = new InvokableMethod<MenuItem>(onSelected.Method);
        }

        public void SetOnItemEnabledChanged(Action<MenuItem> onEnabledChanged)
        {
            EnabledChangedMethod = new InvokableMethod<MenuItem>(onEnabledChanged.Method);
        }

        protected void ValidateAndAttachEventFunctions()
        {
            SelectedItemMethod = new InvokableMethod<MenuItem>(ReflectionHelper.GetMethodWithArguments(OnSelected, new Type[] { typeof(MenuItem) }, EventTargetType));
            EnabledChangedMethod = new InvokableMethod<MenuItem>(ReflectionHelper.GetMethodWithArguments(OnEnabledChanged, new Type[] { typeof(MenuItem) }, EventTargetType));
            ActivatedMethod = new InvokableMethod<MenuItem>(ReflectionHelper.GetMethodWithArguments(OnActivated, new Type[] { typeof(MenuItem) }, EventTargetType));

            AttachEventHandlers();
        }

        protected void AttachEventHandlers()
        {

            Item.Activated += ItemActivated;
            Item.Selected += ItemSelected;
            Item.EnabledChanged += ItemEnabledChanged;
        }

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

        protected void ItemEnabledChanged(object sender, EventArgs e)
        {
            if (EnabledChangedMethod != null)
            {
                EnabledChangedMethod.Invoke(EventTarget, this);
            }
        }

        protected void ItemSelected(object sender, SelectedEventArgs e)
        {
            if (SelectedItemMethod != null)
            {
                SelectedItemMethod.Invoke(EventTarget, this);
            }
        }

        protected void ItemActivated(object sender, EventArgs e)
        {
            if (ActivatedMethod != null)
            {
                ActivatedMethod.Invoke(EventTarget, this);
            }
        }
    }
}
