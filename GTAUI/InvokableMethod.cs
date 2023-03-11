using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI
{
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action"/> delegate.
    /// </summary>
    public class InvokableMethod
    {
        private readonly Action action = null;
        private readonly MethodInfo methodInfo = null;


        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="methodInfo">The MethodInfo object to wrap.</param>
        public InvokableMethod(MethodInfo methodInfo)
        {
            this.methodInfo = methodInfo;
        }

        /// <summary>
        /// Invoke The action if it's not null. if it is, try to invoke the method if that is not null.
        /// </summary>
        /// <param name="target">The target to execute the method on.</param>
        /// <param name="arguments">The arguments to send to the method.</param>
        /// <returns>The return value of the method. Invoking the action will always return null.</returns>
        public object Invoke(object target, object[] arguments)
        {
            if (action != null)
            {
                action();
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, arguments);
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, object[])"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }

    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T}"/> delegate.
    /// </summary>
    public class InvokableMethod<T>
    {
        private readonly Action<T> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T> action)
        {
            this.action = action;
        }

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="methodInfo">The MethodInfo object to wrap.</param>
        public InvokableMethod(MethodInfo methodInfo)
        {
            this.methodInfo = methodInfo;
        }

        /// <summary>
        /// Invoke The action if it's not null. if it is, try to invoke the method if that is not null.
        /// </summary>
        /// <param name="target">The target to execute the method on.</param>
        /// <param name="argument">The argument of the action or method.</param>
        /// <returns>The return value of the method. Invoking the action will always return null.</returns>
        public object Invoke(object target, T argument)
        {
            if (action != null)
            {
                action(argument);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }

    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T1, T2}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2>
    {
        private readonly Action<T, T2> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2> action)
        {
            this.action = action;
        }

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="methodInfo">The MethodInfo object to wrap.</param>
        public InvokableMethod(MethodInfo methodInfo)
        {
            this.methodInfo = methodInfo;
        }

        /// <summary>
        /// Invoke The action if it's not null. if it is, try to invoke the method if that is not null.
        /// </summary>
        /// <param name="target">The target to execute the method on.</param>
        /// <param name="argument1">The first argument of the action or method.</param>
        /// <param name="argument2">The second argument of the action or method.</param>
        /// <returns>The return value of the method. Invoking the action will always return null.</returns>
        public object Invoke(object target, T argument1, T2 argument2)
        {
            if (action != null)
            {
                action(argument1, argument2);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument1, argument2 });
            }
            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
}
