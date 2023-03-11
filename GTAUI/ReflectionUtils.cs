using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTAUI
{
    /// <summary>
    /// Contains helper functions for reflection.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Gets a method info object representing the given method with the given arguments that can be called on objects that are of the given type.
        /// This method logs a message in the GTAUI log when parameters do not match.
        /// </summary>
        /// <param name="functionName">The name of the function to find.</param>
        /// <param name="arguments">The argument types the function must have. Also specifies the amount of arguments.</param>
        /// <param name="targetType">The type to search for methods.</param>
        /// <returns>A method info object representing the method or null if no method was found or the arguments are invalid.</returns>
        public static MethodInfo GetMethodWithArguments(string functionName, Type[] arguments, Type targetType)
        {
            MethodInfo method = targetType.GetMethods().FirstOrDefault(m => m.Name == functionName);
            if (method == null)
            {
                return null;
            }

            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length != arguments.Length)
            {
                UIController.Log($"Invalid method signature: method '{functionName}' expected {arguments.Length} parameters, got {parameters.Length}.");
                return null;
            }

            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i] != parameters[i].ParameterType)
                {
                    UIController.Log($"Invalid method signature: method '{functionName}' expected parameter {i + 1} to be of type {arguments[i]} but the type of the method argument is {parameters[i].ParameterType}.");
                    return null;
                }
            }

            return method;
        }

        /// <summary>
        /// Gets a method info object representing the given method with the given return type that can be called on objects that are of the given type.
        /// This method logs a message in the GTAUI log when the return type do not match.
        /// </summary>
        /// <param name="functionName">The name of the method to search for.</param>
        /// <param name="returnType">The type that the method must return.</param>
        /// <param name="targetType">The type that the method must be defined on.</param>
        /// <returns>A method info object representing the method or null if no method was found or the arguments are invalid.</returns>
        public static MethodInfo GetMehodWithReturnType(string functionName, Type returnType, Type targetType)
        {
            MethodInfo method = targetType.GetMethods().FirstOrDefault(m => m.Name == functionName);
            if (method == null)
            {
                return null;
            }

            if (method.ReturnType != returnType) {
                UIController.Log($"Invalid method signature: method '{functionName}' expected a returntype of {returnType} but got {method.ReturnType}");
                return null;
            }

            return method;
        }
    }
}
