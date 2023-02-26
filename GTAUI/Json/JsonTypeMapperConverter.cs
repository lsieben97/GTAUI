using System;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.CodeDom;
using System.Collections;

namespace GTAUI.Json
{
    /// <summary>
    /// Converts list properties to derived types based on the mappings defined by the <see cref="JsonTypeMapper"/>.
    /// </summary>
    public class JsonTypeMapperConverter : JsonConverter
    {
        /// <inheritdoc/>
        public override bool CanWrite => false;

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!objectType.IsGenericType || objectType.GetGenericTypeDefinition() != typeof(List<>))
            {
                UIController.Log($"JsonTypemapConverter: property is not a {typeof(List<>)}. Skipping JsonTypemap conversion.");
                return serializer.Deserialize(reader);
            }

            Type genericType = objectType.GetGenericArguments()[0];


            var array = JArray.Load(reader);

            PropertyInfo typeProperty = genericType.GetProperties().FirstOrDefault(p => Attribute.IsDefined(p, typeof(JsonTypeMapAttribute)));

            if (typeProperty.PropertyType != typeof(string) && typeProperty.PropertyType.IsEnum == false)
            {
                UIController.Log($"JsonTypemapConverter: property {typeProperty.Name} is not a string property nor is it an enum. Skipping JsonTypemap conversion.");
                return serializer.Deserialize(reader);
            }

            JsonTypeMapAttribute attribute = typeProperty.GetCustomAttribute<JsonTypeMapAttribute>();

            int counter = 0;
            foreach (JObject obj in array.Children<JObject>())
            {
                string type = obj.Children<JProperty>().FirstOrDefault(p => p.Name == typeProperty.Name)?.Value.ToString();
                if (type == null && attribute.DefaultType == null)
                {
                    UIController.Log($"JsonTypemapConverter: object with index {counter} does not have a property named {typeProperty.Name} and there is no default type set on the JsonTypeMap attribute. Skipping JsonTypemap conversion for this object.");
                    counter++;
                    continue;
                }

                type = type ?? attribute.DefaultType;

                Type actualType = JsonTypeMapper.GetInstance().GetMappedType(attribute.TypeGroupName, type);

                if (actualType == null)
                {
                    UIController.Log($"JsonTypemapConverter: object index {counter}: The type alias {type} could not be resolved on the mapping group {attribute.TypeGroupName}. Skipping JsonTypemap conversion for this object.");
                    counter++;
                    continue;
                }

                object deserializedObject = obj.ToObject(actualType, serializer);
                if (deserializedObject == null)
                {
                    UIController.Log($"JsonTypemapConverter: object index {counter}: The deserialized object was null. Skipping JsonTypemap conversion for this object.");
                    counter++;
                    continue;

                }

                (existingValue as IList).Add(deserializedObject);
                counter++;
            }

            return existingValue;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException("This JsonConverter cannot write json.");
        }
    }
}

