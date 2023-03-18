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
    /// Represents the state of the mouse.
    /// </summary>
    public sealed class MouseState
    {
        /// <summary>
        /// The current position of the mouse.
        /// </summary>
        public PointF CurrentPosition { get; internal set; }

        /// <summary>
        /// The mouse buttons that are currently pressed down.
        /// </summary>
        public MouseButtons MouseButtons { get; internal set; }

        /// <summary>
        /// The direction the user is scrolling. <see cref="GTAUI.ScrollDirection.None"/> if the user is not scrolling.
        /// </summary>
        public ScrollDirection ScrollDirection { get; internal set; }

        internal MouseState()
        {
            CurrentPosition = new PointF();
            MouseButtons = MouseButtons.None;
            ScrollDirection = ScrollDirection.None;
        }
    }
}
