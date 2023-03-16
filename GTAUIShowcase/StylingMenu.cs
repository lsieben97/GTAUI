using GTA.UI;
using GTAUI.Menus;
using GTAUI.Menus.MenuItems;
using GTAUI.Styling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUIShowcase
{
    public class StylingMenu : Menu
    {
        public StylingMenu() : base("GTAUIShowcase.resources.stylingMenu.json")
        {
        }

        public void ApplyLightStyle(MenuItem _)
        {
            UIStyle.GetInstance().ApplyStyle("GTAUIShowcase.resources.lightStyle.json");
            Notification.Show("The light style now has been applied. It will affect certain UI elements.");
        }

        public void ResetStyle(MenuItem _)
        {
            UIStyle.GetInstance().ResetStyleProperties();
            Notification.Show("The default style has been restored. It will affect certain UI elements.");
        }
    }
}
