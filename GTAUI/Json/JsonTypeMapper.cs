using System;
using System.Collections.Generic;
using System.IO;
using GTAUI.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GTAUI
{
    /// <summary>
    /// Manages Json type mappings.
    /// Use <c>[JsonConverter(typeof(JsonTypeMapperConverter))]</c> on a list property with a base type to automatically determine the derived type based on a property on the base type with the <see cref="JsonTypeMapAttribute"/>.
    /// </summary>
    public class JsonTypeMapper
	{
		private static JsonTypeMapper instance;

		private readonly Dictionary<string, Dictionary<string, Type>> typeMaps = new Dictionary<string, Dictionary<string, Type>>();

        /// <summary>
        /// Get the current <see cref="JsonTypeMapper"/> instance.
        /// </summary>
        /// <returns>The current <see cref="JsonTypeMapper"/> instance.</returns>
        public static JsonTypeMapper GetInstance()
		{
			if (instance == null)
			{
				instance = new JsonTypeMapper();
			}

			return instance;
		}

		private JsonTypeMapper()
		{
		}


        /// <summary>
        /// Register a new type mapping for the given <paramref name="mappingGroup"/>. If the <paramref name="mappingGroup"/> does not exist is will be created.
        /// If a mapping with the given <paramref name="alias"/> already exists, it will be skipped.
        /// </summary>
        /// <param name="mappingGroup">The name of the mapping group to add the mapping to.</param>
        /// <param name="alias">The alias of the type.</param>
        /// <param name="type">The type the <paramref name="alias"/> should resolve to.</param>
        /// <exception cref="ArgumentNullException">When any of the parameters are <c>null</c>.</exception>
        public void RegisterTypeMapping(string mappingGroup, string alias, Type type)
		{
            if (alias is null)
            {
                throw new ArgumentNullException(nameof(alias));
            }

            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (mappingGroup is null)
            {
                throw new ArgumentNullException(nameof(mappingGroup));
            }

            if (typeMaps.ContainsKey(mappingGroup) == false)
            {
                typeMaps.Add(mappingGroup, new Dictionary<string, Type>());
            }

            if (typeMaps[mappingGroup].ContainsKey(alias))
			{
				return;
			}

			typeMaps[mappingGroup].Add(alias, type);
		}

        /// <summary>
        /// Register multiple type mappings for the given <paramref name="mappingGroup"/>. If the <paramref name="mappingGroup"/> does not exists it will be created.
        /// If a mapping with an alias already exists, it will be skipped.
        /// </summary>
        /// <param name="mappingGroup">The name of the mapping group to add the mappings to.</param>
        /// <param name="typeMappings">An dictionary of alias -> <see cref="Type"/> mappings.</param>
        /// <exception cref="ArgumentNullException">When any of the parameters are <c>null</c>.</exception>
        public void RegisterTypeMappings(string mappingGroup, Dictionary<string, Type> typeMappings)
		{
            if (mappingGroup is null)
            {
                throw new ArgumentNullException(nameof(mappingGroup));
            }

            if (typeMappings is null)
            {
                throw new ArgumentNullException(nameof(typeMappings));
            }

            if (typeMaps.ContainsKey(mappingGroup) == false)
			{
				typeMaps.Add(mappingGroup, typeMappings);
				return;
			}

			foreach (KeyValuePair<string, Type> mapping in typeMappings)
			{
				if (typeMaps[mappingGroup].ContainsKey(mapping.Key))
				{
					continue;
				}

				typeMaps[mappingGroup].Add(mapping.Key, mapping.Value);
			}
		}

        /// <summary>
        /// Get the type the given <paramref name="alias"/> is mapped to or  <c>null</c> if there is no mapping for the <paramref name="alias"/>.
        /// </summary>
        /// <param name="mappingGroup">The name of the mapping group to search in.</param>
        /// <param name="alias">The alias to search for.</param>
        /// <returns>The type the given <paramref name="alias"/> is mapped to or  <c>null</c> if there is no mapping for the <paramref name="alias"/>.</returns>
		public Type GetMappedType(string mappingGroup, string alias)
		{
			if (typeMaps.ContainsKey(mappingGroup) == false)
			{
				return null;
			}

			if (typeMaps[mappingGroup].ContainsKey(alias) == false)
			{
				return null;
			}

			return typeMaps[mappingGroup][alias];
		}
    }
}

