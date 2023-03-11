using System;
namespace GTAUI.Menus
{
    /// <summary>
    /// Badge information for an menu item.
    /// </summary>
    public class MenuItemBadge
    {
        /// <summary>
        /// The name of the dictionary where the <see cref="Texture"/> can be found.
        /// </summary>
        public string Dictionary { get; set; }

        /// <summary>
        /// The name of the texture to use as the badge icon.
        /// </summary>
        public string Texture { get; set; }
    }
}

