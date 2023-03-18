using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.Internal
{
    internal class ScheduledAction
    {
        MethodInfo method;
        object target;
        object[] args;

        public ScheduledAction(MethodInfo method, object target, object[] args)
        {
            Utils.CheckNotNull(method, "method");
            Utils.CheckNotNull(args, "args");

            this.method = method;
            this.target = target;
            this.args = args;
        }

        public void Invoke()
        {
            method.Invoke(target, args);
        }
    }
}
