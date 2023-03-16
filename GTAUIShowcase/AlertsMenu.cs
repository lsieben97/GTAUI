using GTA.UI;
using GTAUI;
using GTAUI.Menus;
using GTAUI.Menus.MenuItems;
using GTAUI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GTAUIShowcase
{
    public class AlertsMenu : Menu
    {
        private ProgressScreen progressScreen;
        private bool doUpdate = false;
        public AlertsMenu() : base("GTAUIShowcase.resources.alertsMenu.json")
        {
        }

        public void ShowAlertExample(MenuItem _)
        {
            Close();
            new AlertScreen("Info", "This is an example alert!", () => Notification.Show("alert accepted"), () => Notification.Show("alert canceled")).Show();
        }

        public void ShowInputScreenExample(MenuItem _)
        {
            Close();
            new InputScreen("Input", "Please type something below:", (input) => Notification.Show($"You've typed: {input}"), () => Notification.Show("input canceled")).Show();
        }

        public void ShowProgressScreenExample(MenuItem _)
        {
            Close();
            progressScreen = new ProgressScreen("Processing", "Processing something very important...");
            progressScreen.Show();
            UIController.GetInstance().TickHandled += IncrementProgress;
        }

     private void IncrementProgress(object sender, EventArgs e)
        {
            if (progressScreen.CurrentProgress == 100)
            {
                UIController.GetInstance().TickHandled -= IncrementProgress;
                progressScreen.Dispose();
            }
            else if(doUpdate)
            {
                progressScreen.SetProgress(progressScreen.CurrentProgress + 1);
            }

            doUpdate = !doUpdate;
        }
    }
}
