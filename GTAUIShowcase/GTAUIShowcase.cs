using GTA;
using GTA.UI;
using GTAUI;
using GTAUI.Menus;
using GTAUI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GTAUIShowcase
{
    public class GTAUIShowcase : Script
    {
        private readonly UIController uiController;

        public GTAUIShowcase()
        {
            uiController = UIController.Connect(this);
            uiController.KeyDownHandled += OnKeyDown;
            uiController.TickHandled += OnTickHandled;
        }

        private void OnTickHandled(object sender, EventArgs e)
        {
            Notification.Show("GTAUI Showcase loaded. Press U to start.");
            uiController.TickHandled -= OnTickHandled;
        }

        private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == System.Windows.Forms.Keys.U && uiController.IsMenuShowing("MainMenu") == false)
            {
                new MainMenu().Show();
            }
        }
    }
}
