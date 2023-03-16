using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.Styling
{
    public class UIStyle
    {
        private static UIStyle instance;

        private List<StyleProperty> properties = new List<StyleProperty>();

        /// <summary>
        /// Fired when any of the style properties change by calling <see cref="ApplyStyle(string)"/> or <see cref="ResetStyleProperties"/>.
        /// </summary>
        public event EventHandler StylePropertiesChanged;

        public static UIStyle GetInstance()
        {
            if (instance == null)
            {
                instance = new UIStyle();
            }

            return instance;
        }

        /// <summary>
        /// Register the style properties in the given ui resource.
        /// </summary>
        /// <param name="uiResourcePath">The path to the ui resource to load style properties from.</param>
        /// <returns><c>true</c> when the loading of the style properties succeeded, <c>false</c> otherwise.</returns>
        public bool RegisterStylingProperties(string uiResourcePath)
        {
            string json = UIController.instance.GetUIResource(uiResourcePath);
            if (json == null)
            {
                return false;
            }

            try
            {
                List<StyleProperty> styleProperties = JsonConvert.DeserializeObject<List<StyleProperty>>(json);
                List<StyleProperty> propertiesToRemove = new List<StyleProperty>();
                foreach (StyleProperty styleProperty in styleProperties)
                {
                    object actualValue = styleProperty.GetActualValue(styleProperty.DefaultValue);
                    if (actualValue == null)
                    {
                        propertiesToRemove.Add(styleProperty);
                        continue;
                    }

                    styleProperty.ActualValue = actualValue;
                }
                styleProperties.RemoveAll(styleProperty => propertiesToRemove.Contains(styleProperty));
                properties.AddRange(styleProperties);


            }
            catch (Exception ex)
            {
                UIController.Log($"Could not load style properties from {uiResourcePath}:{ex}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Applies the style property values that are in the given ui resource.
        /// </summary>
        /// <param name="uiResourcePath">The path to the ui resource.</param>
        /// <returns><c>true</c> when the loading of the style properties succeeded, <c>false</c> otherwise.</returns>
        public bool ApplyStyle(string uiResourcePath)
        {
            string json = UIController.instance.GetUIResource(uiResourcePath);
            if (json == null)
            {
                return false;
            }

            Dictionary<string, object> styleProperties = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            bool propertiesChanged = false;

            foreach (KeyValuePair<string, object> property in styleProperties)
            {
                StyleProperty existingProperty = properties.Find(p => p.Name == property.Key);
                if (existingProperty == null)
                {
                    UIController.Log($"Skipping style property '{property.Key}' because no style property with that name exists.");
                    continue;
                }

                object actualValue = existingProperty.GetActualValue(property.Value);
                if (actualValue == null)
                {
                    continue;
                }

                existingProperty.ActualValue = actualValue;
                propertiesChanged = true;
            }

            if (propertiesChanged)
            {
                StylePropertiesChanged?.Invoke(this, EventArgs.Empty);
            }

            return true;
        }

        /// <summary>
        /// Get the value of the given style property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="property">The name of the property.</param>
        /// <returns>The value of the property with the given name or <c>default(T)</c> if the property could not be found or the type is incorrect.</returns>
        public T GetStyleProperty<T>(string property)
        {
            StyleProperty propertyObject = properties.Find(p => p.Name == property);
            if (propertyObject == null)
            {
                UIController.Log($"Unable to find style property with the name '{property}'");
                return default(T);
            }

            if (typeof(T) != propertyObject.ActualType)
            {
                UIController.Log($"Invalid generic type argument given. Expected {propertyObject.ActualType}, got {typeof(T)}.");
                return default(T);
            }

            return (T)propertyObject.ActualValue;
        }

        /// <summary>
        /// Dump all registered style properties and their values to the GTAUI Log file.
        /// </summary>
        public void DumpStyleProperties()
        {
            StringBuilder builder = new StringBuilder();
            foreach(StyleProperty property in properties)
            {
                builder.AppendLine($"{property.Name}\t\t({property.ActualType})\t\t{property.ActualValue}");
            }

            UIController.Log("Registered style properties:\n" + builder.ToString());
        }

        /// <summary>
        /// Set the value of all style properties to their default values.
        /// </summary>
        public void ResetStyleProperties()
        {
            foreach(StyleProperty property in properties)
            {
                property.ActualValue = property.GetActualValue(property.DefaultValue);
            }
        }
    }
}
