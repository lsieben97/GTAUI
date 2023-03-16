using GTAUI;
using GTAUI.Menus;
using GTAUI.Menus.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GTAUIShowcase
{
    public class AboutMenu : Menu
    {
        public AboutMenu() : base("About", string.Empty) 
        {
            
        }

        protected override bool BeforeShow()
        {

            SubTitle = "Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            ButtonMenuItem developerItem = new ButtonMenuItem(string.Empty, "Developed by Luc Sieben", (_) => { });
            developerItem.Item.Enabled = false;
            AddMenuItem(developerItem);

            ButtonMenuItem gtaUIVersionItem = new ButtonMenuItem(string.Empty, "Using GTAUI version " + UIController.GetInstance().GTAUIVersion, (_) => { });
            gtaUIVersionItem.Item.Enabled = false;
            AddMenuItem(gtaUIVersionItem);

            AddMenuItem(new BackMenuItem());
            return true;
        }
    }
}
