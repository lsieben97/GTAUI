using GTAUI;
using LemonUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UITest
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

        public override void OnInitialize()
        {
            NeedsGameControlsDisabled = true;
            NeedsVisibleMouseCursor = true;
            text = new ScaledText(new PointF(10, 10), "", 0.4f);
            text.Color = Color.Red;
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            keyDown = e.KeyCode.ToString();
        }

        public override void OnKeyUp(KeyEventArgs e)
        {
            keyUp = e.KeyCode.ToString();
        }

        public override void OnMouseButtonDown(MouseButtons buttons)
        {
            leftMouseButton = buttons == MouseButtons.Left;
            rightMouseButton = buttons == MouseButtons.Right;
        }

        public override void OnMouseMove(System.Drawing.PointF position)
        {
            mousePosition = position;
        }

        public override void OnMouseScroll(ScrollDirection direction)
        {
            mouseScrollUp = direction == ScrollDirection.Up;
            mouseScrollDown = direction == ScrollDirection.Down;
        }

        public override void Render()
        {
            text.Draw();
        }

        public override void Update()
        {
            text.Text = $"Mouse position: {mousePosition}\nkeyDown: {keyDown}\nkeyUp: {keyUp}\nleft mouse button pressed: {leftMouseButton}\nright mouse button pressed: {rightMouseButton}\nmouse scrolling down: {mouseScrollDown}\nmouse scrolling up: {mouseScrollUp}";
            mouseScrollDown = false;
            mouseScrollUp = false;
        }
    }
}
