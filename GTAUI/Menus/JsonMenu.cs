using GTAUI.Json;
using GTAUI.Menus.MenuItems;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.Menus
{
    /// <summary>
    /// Represents a json definition of a menu.
    /// For internal use only
    /// </summary>
    public class JsonMenu
    {
        /// <summary>
        /// The title of the menu.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The sub title of the menu.
        /// </summary>
        public string SubTitle { get; set; } = string.Empty;

        /// <summary>
        /// The menu items that are in this menu.
        /// </summary>
        [JsonConverter(typeof(JsonTypeMapperConverter))]
        public List<MenuItem> Items = new List<MenuItem>();
    }
}
