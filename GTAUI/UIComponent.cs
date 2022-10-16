using LemonUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTAUI
{
    public abstract class UIComponent : IDisposable
    {
        public bool Visible { get; set; } = false;
        public SizeF Size { get; set; } = new SizeF();
        public PointF Position { get; set; } = new PointF();
        public bool IsFullScreen { get; set; } = false;
        public bool NeedsGameControlsDisabled { get; set; } = false;
        public bool NeedsVisibleMouseCursor { get; set; } = false;
        internal bool IsDisposed { get; set; } = false;

        protected UIComponent Parent { get; private set; } = null;

        internal List<UIComponent> ChildComponents = new List<UIComponent>();

        public UIComponent()
        {
            if (IsFullScreen)
            {
                Size = new SizeF(UIController.instance.ScreenSize.Width, UIController.instance.ScreenSize.Height);
                Position = new PointF(0, 0);
            }
            UIController.instance.AddComponent(this);
        }

        internal void FireKeyDown(KeyEventArgs e)
        {
            OnKeyDown(e);
            foreach (UIComponent component in ChildComponents)
            {
                component.FireKeyDown(e);
            }
        }

        public virtual void OnKeyDown(KeyEventArgs e)
        {
        }

        internal void FireKeyUp(KeyEventArgs e)
        {
            OnKeyUp(e);
            foreach (UIComponent component in ChildComponents)
            {
                component.FireKeyUp(e);
            }
        }

        public virtual void OnKeyUp(KeyEventArgs e)
        {
        }

        internal void FireInitialize()
        {
            OnInitialize();
            foreach (UIComponent component in ChildComponents)
            {
                component.FireInitialize();
            }
        }

        public virtual void OnInitialize()
        {
        }

        internal void FireDisposed()
        {
            OnDisposed();
            foreach (UIComponent component in ChildComponents)
            {
                component.FireDisposed();
            }
        }

        public virtual void OnDisposed()
        {
        }

        internal void FireMouseMove(PointF position)
        {
            OnMouseMove(position);
            foreach (UIComponent component in ChildComponents)
            {
                component.FireMouseMove(position);
            }
        }

        public virtual void OnMouseMove(PointF position)
        {
        }

        internal void FireMouseButtonDown(MouseButtons buttons)
        {
            OnMouseButtonDown(buttons);
            foreach (UIComponent component in ChildComponents)
            {
                component.FireMouseButtonDown(buttons);
            }
        }

        public virtual void OnMouseButtonDown(MouseButtons buttons)
        {
        }

        internal void FireMouseScroll(ScrollDirection direction)
        {
            OnMouseScroll(direction);
            foreach (UIComponent component in ChildComponents)
            {
                component.FireMouseScroll(direction);
            }
        }

        public virtual void OnMouseScroll(ScrollDirection direction)
        {
        }

        internal void FireUpdate()
        {
            Update();
            foreach (UIComponent component in ChildComponents)
            {
                component.FireUpdate();
            }
        }

        public virtual void Update()
        {
        }

        internal void FireRender()
        {
            Render();
            foreach (UIComponent component in ChildComponents)
            {
                component.Render();
            }
        }

        public virtual void Show()
        {
            Visible = true;
        }

        public virtual void Hide()
        {
            Visible = false;
        }

        public void AddChildComponent(UIComponent component)
        {
            component.Parent = this;
            ChildComponents.Add(component);
        }

        public PointF GetActualPosition(PointF position)
        {
            UIComponent currentComponent = this;
            PointF actualPosition = new PointF();
            while (currentComponent != null)
            {
                actualPosition.X += currentComponent.Position.X;
                actualPosition.Y += currentComponent.Position.Y;
                currentComponent = currentComponent.Parent;
            }

            return actualPosition;
        }

        public abstract void Render();

        public void Dispose()
        {
            FireDisposed();
            UIController.instance.RemoveComponent(this);
        }
    }
}
