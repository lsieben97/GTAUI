using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.Menus
{
    /// <summary>
    /// Represents an object that can be selected in a menu.
    /// </summary>
    public interface IMenuSelectable
    {
        /// <summary>
        /// Get a non null string that will be the title of this object in the menu for this object.
        /// </summary>
        /// <returns>A non null string that will be the title of this object in the menu for this object.</returns>
        string GetMenuItemTitle();

        /// <summary>
        /// Get a string that will be the description of this object in the menu for this object.
        /// </summary>
        /// <returns>A string that will be the description of this object in the menu for this object.</returns>
        string GetMenuItemDescription();

        /// <summary>
        /// Return wether the menu item for this object should be enabled.
        /// </summary>
        /// <returns><c>true</c> if the menu should be enabled, <c>false</c> otherwise.</returns>
        bool IsMenuItemEnabled();
    }
}
