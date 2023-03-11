using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.Menus
{
    public class JsonMenuItem
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "Title";
        public string Description { get; set; } = null;
        public string OnSelected { get; set; }
        public string OnActivated { get; set; }
        public string OnEnabledChanged { get; set; }
        public string OnCheckboxChanged { get; set; }
        public string OnItemSelected { get; set; }
        public string GetListFunc { get; set; }
        public int Maximum { get; set; } = 0;
        public string GetMaximumFunc { get; set; }
        public string SubmenuName { get; set; }
        public string GetSubmenuFunc { get; set; }
        public JsonMenuItemBadge RightBadge { get; set; } = null;
        public JsonMenuItemBadge LeftBadge { get; set; } = null;
        public MenuItemType Type { get; set; } = MenuItemType.Button;
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MenuItemType
    {
        Button,
        Checkbox,
        List,
        Slider,
        Close,
        Back,
        Submenu
    }
}
