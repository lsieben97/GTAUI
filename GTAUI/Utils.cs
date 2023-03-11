using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI
{
    public static class Utils
    {
        /// <summary>
        /// Checks if the given object is null. If so, it throws a <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="target">The object to check.</param>
        /// <param name="name">The name of the variable to report when throwing the exception.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="target"/> is null.</exception>
        public static void CheckNotNull(object target, string name)
        {
            if (target == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
