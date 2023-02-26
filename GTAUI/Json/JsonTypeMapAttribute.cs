using System;
namespace GTAUI.Json
{
	/// <summary>
	/// Specifies that the property with this attribute defines the alias of a type resolved by the <see cref="JsonTypeMapper"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class JsonTypeMapAttribute : Attribute
	{
		public string TypeGroupName { get; set; }
		public string DefaultType { get; set; }

        /// <summary>
        /// Create a new instance of this attribute with the given <paramref name="typeGroupName"/> and optionally an alias to use when the value of the property that has this attribute is <c>null</c>.
        /// </summary>
        /// <param name="typeGroupName">The name of the mapping group to use.</param>
        /// <param name="defaultType">An alias to use when the value of the property that has this attribute is <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="typeGroupName"/> is <c>null</c>.</exception>
        public JsonTypeMapAttribute(string typeGroupName, string defaultType = "")
		{
            if (typeGroupName is null)
            {
                throw new ArgumentNullException(nameof(typeGroupName));
            }

			TypeGroupName = typeGroupName;
			DefaultType = defaultType;
        }
	}
}
