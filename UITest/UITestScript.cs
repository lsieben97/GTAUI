using GTA;
using GTAUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITest
{
    public class UITestScript : Script
    {
        private UIController controller;
        public UITestScript()
        {
            GTA.UI.Notification.Show("UI test!");
            controller = new UIController();
            controller.Initialize();
            KeyDown += UITestScript_KeyDown;
            KeyUp += UITestScript_KeyUp;
            Tick += UITestScript_Tick;
            DebugComponent debug = new DebugComponent();
            debug.Show();
        }

        private void UITestScript_Tick(object sender, EventArgs e)
        {
            controller.OnTick();
        }

        private void UITestScript_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            controller.OnKeyUp(e);
        }

        private void UITestScript_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            controller.OnKeyDown(e);
        }
    }
}
