﻿using LemonUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTAUI
{
    /// <summary>
    /// The base class for any object drawing UI and listening to keyboard and mouse events.
    /// </summary>
    public abstract class UIComponent : IDisposable
    {
        /// <summary>
        /// <c>true</c> if the component should be drawn by the <see cref="UIController"/>. <c>false</c> otherwise.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// The size of the component. Not used by the <see cref="UIController"/> but can be used when measuring child components.
        /// </summary>
        public SizeF Size { get; set; } = new SizeF();

        /// <summary>
        /// The position of the component. 0,0 is the top left corner. Not used by the <see cref="UIController"/> but handy to use in combination with <see cref="GetActualPosition(PointF)"/>.
        /// </summary>
        public PointF Position { get; set; } = new PointF();

        /// <summary>
        /// The bounds of the component, created with the <see cref="Position"/> and <see cref="Size"/> properties.
        /// </summary>
        public RectangleF Bounds { get => new RectangleF(Position, Size); }

        /// <summary>
        /// <c>true</c> if the component has the same size as the screen. Setting this to <c>true</c> causes the component to automatically set the <see cref="Size"/> to the same value as <see cref="UIController.ScreenSize"/>.
        /// </summary>
        public bool IsFullScreen { get; set; } = false;

        /// <summary>
        /// <c>true</c> when the component needs all game controls to be disabled as long as the component is visible. Will be taken care of by the <see cref="UIController"/>.
        /// </summary>
        public bool NeedsGameControlsDisabled { get; set; } = false;

        /// <summary>
        /// <c>true</c> if the component needs the mouse cursor to be visible as long as the component is visible. Will be taken care off by the <see cref="UIController"/>.
        /// Note: mouse events will still be fired even if the mouse cursor is invisible. (as long as the component is visible)
        /// </summary>
        public bool NeedsVisibleMouseCursor { get; set; } = false;

        /// <summary>
        /// <c>true</c> if the component only needs mouse events when the mouse is in the <see cref="Bounds"/> of the component and no other component is on top of this component.
        /// </summary>
        public bool NeedsPositionalMouseEvents { get; set; } = false;

        /// <summary>
        /// <c>true</c> if the component has focus. Not used by the <see cref="UIController"/> but handy for determining whether the component needs to handle keyboard and mouse events.
        /// </summary>
        public bool HasFocus { get; set; }

        /// <summary>
        /// <c>true</c> if the component always needs mouse and keyboard input events sent to it, even if it's not visible.
        /// </summary>
        public bool AlwaysNeedsInput { get; set; } = false;

        /// <summary>
        /// <c>true</c> if the <see cref="UIController"/> should block user input for a small amount of time when the component is registered.
        /// This is useful if you don't want this component to respond to previous user input like accepting a menu or typing in a textbox.
        /// </summary>
        public bool NeedsStartTimeout { get; protected set; }

        /// <summary>
        /// <c>true</c> if the timeout specified by <see cref="NeedsStartTimeout"/> has elapsed. For components that do not have a start timeout, this property will always be <c>true</c>.
        /// </summary>
        public bool StartTimeoutFinished { get; internal set; }

        internal int StartTimeoutFrames { get; set; }

        internal bool IsDisposed { get; set; } = false;

        internal bool IsInitialized { get; set; } = false;

        internal bool wasMouseInBounds { get; set; } = false;
        internal bool wasInvisible { get; set; } = true;

        /// <summary>
        /// The parent of the component or <c>null</c> if there is no parent.
        /// </summary>
        protected UIComponent Parent { get; private set; } = null;

        internal List<UIComponent> ChildComponents = new List<UIComponent>();

        /// <summary>
        /// Base constructor. Sets the <see cref="Size"/> and <see cref="Position"/> properties if the <see cref="IsFullScreen"/> property is <c>true</c>.
        /// </summary>
        public UIComponent()
        {
            if (IsFullScreen)
            {
                Size = new SizeF(UIController.GetInstance().ScreenSize.Width, UIController.GetInstance().ScreenSize.Height);
                Position = new PointF(0, 0);
            }
        }

        internal void FireKeyDown(KeyEventArgs e)
        {
            OnKeyDown(e);

            foreach (UIComponent component in ChildComponents)
            {
                component.FireKeyDown(e);
            }
        }

        /// <summary>
        /// This method is called every time a key is pressed down. After this method returns, it will also be called on all child components if there are any.
        /// </summary>
        /// <param name="e">The Event arguments</param>
        protected virtual void OnKeyDown(KeyEventArgs e)
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

        /// <summary>
        /// This method is called every time a key is lifted up. After this method returns, it will also be called on all child components if there are any.
        /// </summary>
        /// <param name="e">The Event arguments</param>
        protected virtual void OnKeyUp(KeyEventArgs e)
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

        /// <summary>
        /// This method is called by the <see cref="UIController"/> after the component is added to the list of components to handle. After this method returns, it will also be called on all child components if there are any.
        /// </summary>
        protected virtual void OnInitialize()
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

        /// <summary>
        /// This method is called by the <see cref="UIController"/> after the component is removed from the list of components to handle. After this method returns, it will also be called on all child components if there are any.
        /// </summary>
        protected virtual void OnDisposed()
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
        /// <summary>
        /// This method is called by the <see cref="UIController"/> when the mouse position is different from the last frame.
        /// </summary>
        /// <param name="position">The new position of the mouse.</param>
        protected virtual void OnMouseMove(PointF position)
        {
        }

        internal void FireMouseButtonDown(MouseButtons buttons)
        {
            OnMouseButtonChange(buttons);
            foreach (UIComponent component in ChildComponents)
            {
                component.FireMouseButtonDown(buttons);
            }
        }

        /// <summary>
        /// This method is called by the <see cref="UIController"/> when any of the mouse button states is different from the last frame.
        /// If the user presses the left mouse down, holds it and releases it, this method will be called 2 times, first with <see cref="MouseButtons.Left"/> and secondly with <see cref="MouseButtons.None"/> 
        /// </summary>
        /// <param name="buttons">The new mouse button state</param>
        protected virtual void OnMouseButtonChange(MouseButtons buttons)
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

        /// <summary>
        /// This method is called every frame the mouse is scrolling.
        /// </summary>
        /// <param name="direction">The direction the mouse is scrolling.</param>
        protected virtual void OnMouseScroll(ScrollDirection direction)
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

        internal void FireMouseEnter(PointF position)
        {
            OnMouseEnter(position);
            foreach (UIComponent component in ChildComponents)
            {
                component.FireMouseEnter(position);
            }
        }

        /// <summary>
        /// This method is called on the frame the mouse cursor enters the <see cref="Bounds"/> of the component.
        /// Will only be called when <see cref="NeedsPositionalMouseEvents"/> is set to <c>true</c>.
        /// </summary>
        /// <param name="position">The position of the mouse.</param>
        protected virtual void OnMouseEnter(PointF position)
        {

        }

        internal void FireMouseLeave(PointF position)
        {
            OnMouseLeave(position);
            foreach (UIComponent component in ChildComponents)
            {
                component.FireMouseLeave(position);
            }
        }

        /// <summary>
        /// This method is called on the frame the mouse cursor leaves the <see cref="Bounds"/> of the component.
        /// Will only be called when <see cref="NeedsPositionalMouseEvents"/> is set to <c>true</c>.
        /// </summary>
        /// <param name="position">The position of the mouse.</param>
        protected virtual void OnMouseLeave(PointF position)
        {

        }

        /// <summary>
        /// This method is called every frame the component is handled by the <see cref="UIController"/>.
        /// </summary>
        protected virtual void Update()
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

        /// <summary>
        /// Allows the <see cref="UIController"/> to render the component as long as <see cref="Hide"/> is not called and <see cref="Visible"/> is <c>true</c>.
        /// </summary>
        public virtual void Show()
        {
            Visible = true;
        }

        /// <summary>
        /// Stops the <see cref="UIController"/> from rendering the component but keeps checking every frame if it can render again.
        /// </summary>
        public virtual void Hide()
        {
            Visible = false;
        }

        /// <summary>
        /// Add the given component as a child of the current component.
        /// </summary>
        /// <param name="component">The component to add.</param>
        public void AddChildComponent(UIComponent component)
        {
            component.Parent = this;
            ChildComponents.Add(component);
        }

        /// <summary>
        /// Register the component with the <see cref="UIController"/>. Without this call the component will not receive any events nor is it able to update or render.
        /// </summary>
        public void Register()
        {
            UIController.GetInstance().AddComponent(this);
        }

        /// <summary>
        /// Translate a point withing the bounds of the component to an absolute position in screen coordinates by adding the given position to the position of one or more parent components. 
        /// </summary>
        /// <param name="position">The position to translate.</param>
        /// <returns>The translated position.</returns>
        public PointF GetActualPosition(PointF position)
        {
            UIComponent currentComponent = this;
            PointF actualPosition = position;
            while (currentComponent != null)
            {
                actualPosition.X += currentComponent.Position.X;
                actualPosition.Y += currentComponent.Position.Y;
                currentComponent = currentComponent.Parent;
            }

            return actualPosition;
        }

        /// <summary>
        /// This method is called every frame the component is handled by the <see cref="UIController"/> and the <see cref="Visible"/> property is <c>true</c>.
        /// </summary>
        protected abstract void Render();

        /// <summary>
        /// Remove the component from the list of components managed by the <see cref="UIController"/>
        /// In order for the component to work again <see cref="Register"/> must be called.
        /// </summary>
        public void Dispose()
        {
            FireDisposed();
            UIController.GetInstance().RemoveComponent(this);
        }
    }
}
