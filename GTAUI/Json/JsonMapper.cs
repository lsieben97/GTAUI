using System;
using System.Collections.Generic;
using System.IO;
using GTAUI.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GTAUI
{
	public class JsonMapper
	{
		private static JsonMapper instance;

		private Dictionary<string, Dictionary<string, Type>> typeMaps = new Dictionary<string, Dictionary<string, Type>>();


		public static JsonMapper GetInstance()
		{
			if (instance == null)
			{
				instance = new JsonMapper();
			}

			return instance;
		}

		private JsonMapper()
		{
		}

		public T DeserializeObject<T>(string jsonString)
		{
			JsonSerializerSettings settings = JsonConvert.DefaultSettings?.Invoke();
			if (settings == null)
			{
				settings = new JsonSerializerSettings();
			}

			settings.Converters.Add(new JsonTypeMapperConverter());

			JsonSerializer serializer = JsonSerializer.Create(settings);

			return serializer.Deserialize<T>(new JsonTextReader(new StringReader(jsonString)));
		}

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

