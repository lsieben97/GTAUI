using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTAUI
{
    /// <summary>
    /// Extension methods for GTAUI.
    /// </summary>
    public static class Extensions
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int MapVirtualKey(int uCode, int uMapType);

        /// <summary>
        /// Checks wether the key represents a character that can be printed visibly on screen.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns><c>true</c> when the key represents a character that can be printed visibly on screen. <c>false</c> otherwise.</returns>
        public static bool RepresentsPrintableChar(this Keys key)
        {
            return !char.IsControl((char)MapVirtualKey((int)key, 2));
        }

        /// <summary>
        /// Get the string represented by the key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The string represented by the key.</returns>
        public static string GetString(this Keys key)
        {
            return ((char)MapVirtualKey((int)key, 2)).ToString();
        }
    }
}
