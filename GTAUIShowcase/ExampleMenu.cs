using GTA.UI;
using GTAUI.Menus;
using GTAUI.Menus.MenuItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUIShowcase
{
    public class ExampleMenu : Menu
    {
        public ExampleMenu() : base("GTAUIShowcase.resources.testMenu.json")
        {
        }

        public void ItemSelected(MenuItem item)
        {
            Notification.Show($"Selected '{item.Title}'");
        }

        public void ButtonActivated(MenuItem _)
        {
            Notification.Show("You activated a button");
        }

        public void CheckboxChanged(MenuItem _, bool newValue)
        {
            Notification.Show($"New checkbox value: {newValue}");
        }

        public IEnumerable<object> GetList()
        {
            return new List<string>() { "Apples", "Pears", "Bananas" };
        }

        public void ListItemSelected(MenuItem _, object newSelection) {
            Notification.Show($"New selected value: {newSelection}");
        }

        public void SliderItemSelected(MenuItem _, int newSelection)
        {
            Notification.Show($"New selected number: {newSelection}");
        }

    }
}
