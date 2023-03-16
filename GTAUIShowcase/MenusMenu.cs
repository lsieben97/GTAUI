using GTA.UI;
using GTAUI.Menus;
using GTAUI.Menus.MenuItems;
using System;
using System.Collections.Generic;

namespace GTAUIShowcase
{
    public class MenusMenu : Menu
    {
        public MenusMenu() : base("GTAUIShowcase.resources.menusMenu.json")
        {
            
        }
        public void ShowSelectMenu(MenuItem _)
        {
            Close();
            new SelectionMenu<string>("Example Select",
                "Please make a selection",
                new List<string>() { "Apples", "Pears", "Bananas", "Grapes", "Oranges", "Kiwis" },
                (selection) => { Notification.Show($"You selected: '{selection}'"); },
                () => { Notification.Show($"Selection menu canceled."); }).Show();
;        }

        public void ShowMultiSelectMenu(MenuItem _)
        {
            Close();
            new MultiSelectionMenu<string>("Example MultiSelect",
                "Please make a selection",
                new List<string>() { "Apples", "Pears", "Bananas", "Grapes", "Oranges", "Kiwis" },
                (selection) => {
                    string total = string.Join(", ", selection);
                    Notification.Show($"You selected: {total}");
                },
                () => { Notification.Show($"Selection menu canceled."); }).Show();
        }
    }
}
