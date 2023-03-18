using System;
using System.Reflection;

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
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2}"/> delegate.
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
        /// <param name="argument">The argument of the action or method.</param>
        /// <returns>The return value of the method. Invoking the action will always return null.</returns>
        public object Invoke(object target, T argument, T2 argument2)
        {
            if (action != null)
            {
                action(argument, argument2);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2 });
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
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3>
    {
        private readonly Action<T, T2, T3> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3)
        {
            if (action != null)
            {
                action(argument, argument2, argument3);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4>
    {
        private readonly Action<T, T2, T3, T4> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4, T5}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4, T5>
    {
        private readonly Action<T, T2, T3, T4, T5> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4, T5> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4, T5 argument5)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4, argument5);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4, argument5 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4, T5)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4, T5, T6}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4, T5, T6>
    {
        private readonly Action<T, T2, T3, T4, T5, T6> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4, T5, T6> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4, argument5, argument6);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4, argument5, argument6 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4, T5, T6)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4, T5, T6, T7}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4, T5, T6, T7>
    {
        private readonly Action<T, T2, T3, T4, T5, T6, T7> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4, T5, T6, T7> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4, argument5, argument6, argument7);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4, argument5, argument6, argument7 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4, T5, T6, T7)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4, T5, T6, T7, T8}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4, T5, T6, T7, T8>
    {
        private readonly Action<T, T2, T3, T4, T5, T6, T7, T8> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4, T5, T6, T7, T8> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4, T5, T6, T7, T8)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4, T5, T6, T7, T8, T9}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        private readonly Action<T, T2, T3, T4, T5, T6, T7, T8, T9> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4, T5, T6, T7, T8, T9> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4, T5, T6, T7, T8, T9)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        private readonly Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4, T5, T6, T7, T8, T9, T10)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {
        private readonly Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        private readonly Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {
        private readonly Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {
        private readonly Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {
        private readonly Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14, T15 argument15)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    
    /// <summary>
    /// Wrapper class for either a <see cref="MethodInfo"/> object or an <see cref="Action{T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}"/> delegate.
    /// </summary>
    public class InvokableMethod<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    {
        private readonly Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action;
        private readonly MethodInfo methodInfo = null;

        /// <summary>
        /// Create a new InvokableMethod with the given action.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        public InvokableMethod(Action<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
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
        public object Invoke(object target, T argument, T2 argument2, T3 argument3, T4 argument4, T5 argument5, T6 argument6, T7 argument7, T8 argument8, T9 argument9, T10 argument10, T11 argument11, T12 argument12, T13 argument13, T14 argument14, T15 argument15, T16 argument16)
        {
            if (action != null)
            {
                action(argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15, argument16);
                return null;
            }
            else if (methodInfo != null)
            {
                return methodInfo.Invoke(target, new object[] { argument, argument2, argument3, argument4, argument5, argument6, argument7, argument8, argument9, argument10, argument11, argument12, argument13, argument14, argument15, argument16 });
            }

            return null;
        }

        /// <summary>
        /// Check whether the wrapped action and method are both null.
        /// If this returns <c>true</c>, calling the <see cref="Invoke(object, T, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)"/> method will do nothing.
        /// </summary>
        /// <returns><c>true</c> if both the wrapped action and method are null.</returns>
        public bool IsNull()
        {
            return action == null && methodInfo == null;
        }
    }
    }
