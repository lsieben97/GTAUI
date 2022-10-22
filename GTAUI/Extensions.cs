using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTAUI
{
    public static class Extensions
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int MapVirtualKey(int uCode, int uMapType);

        public static bool RepresentsPrintableChar(this Keys key)
        {
            return !char.IsControl((char)MapVirtualKey((int)key, 2));
        }

        public static string GetString(this Keys key)
        {
            return ((char)MapVirtualKey((int)key, 2)).ToString();
        }
    }
}
