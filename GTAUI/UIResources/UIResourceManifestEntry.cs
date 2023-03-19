using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.UIResources
{
    /// <summary>
    /// Represents a manifest entry for a ui resource.
    /// </summary>
    public sealed class UIResourceManifestEntry
    {
        /// <summary>
        /// The name of the ui resource.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of the ui resource.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The manifest definition. Useful for getting type specific properties.
        /// </summary>
        public JObject ManifestDefinition { get; set; }

        /// <summary>
        /// Create a new manifest entry.
        /// </summary>
        /// <param name="name">The name of the ui resource</param>
        /// <param name="type">The type of the ui resource.</param>
        /// <param name="manifestDefinition">The manifest definition.</param>
        public UIResourceManifestEntry(string name, string type, JObject manifestDefinition)
        {
            Name = name;
            Type = type;
            ManifestDefinition = manifestDefinition;
        }

        /// <summary>
        /// Get a type specific property from the <see cref="ManifestDefinition"/>.
        /// </summary>
        /// <typeparam name="T">The type of the property to get</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of the property or null if it was not found or was of the wrong type.</returns>
        public T GetProperty<T>(string propertyName)
        {
            return ManifestDefinition.Value<T>(propertyName);
        }
    }
}
