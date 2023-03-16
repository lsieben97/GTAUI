using GTAUI;
using LemonUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTAUIShowcase
{
    public class DebugComponent : UIComponent
    {
        private ScaledText text;
        private string keyDown;
        private string keyUp;
        private bool leftMouseButton;
        private bool mouseScrollUp;
        private bool mouseScrollDown;
        private bool rightMouseButton;
        private PointF mousePosition;

        protected override void OnInitialize()
        {
            NeedsGameControlsDisabled = true;
            NeedsVisibleMouseCursor = true;
            text = new ScaledText(new PointF(10, 10), "", 0.4f);
            text.Color = Color.Red;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            keyDown = e.KeyCode.ToString();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            keyUp = e.KeyCode.ToString();
        }

        protected override void OnMouseButtonChange(MouseButtons buttons)
        {
            leftMouseButton = buttons == MouseButtons.Left;
            rightMouseButton = buttons == MouseButtons.Right;
        }

        protected override void OnMouseMove(System.Drawing.PointF position)
        {
            mousePosition = position;
        }

        protected override void OnMouseScroll(ScrollDirection direction)
        {
            mouseScrollUp = direction == ScrollDirection.Up;
            mouseScrollDown = direction == ScrollDirection.Down;
        }

        protected override void Render()
        {
            text.Draw();
        }

        protected override void Update()
        {
            text.Text = $"Mouse position: {mousePosition}\nkeyDown: {keyDown}\nkeyUp: {keyUp}\nleft mouse button pressed: {leftMouseButton}\nright mouse button pressed: {rightMouseButton}\nmouse scrolling down: {mouseScrollDown}\nmouse scrolling up: {mouseScrollUp}";
            mouseScrollDown = false;
            mouseScrollUp = false;
        }
    }
}
