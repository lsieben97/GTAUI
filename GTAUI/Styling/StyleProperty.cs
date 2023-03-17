using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.Styling
{
    internal class StyleProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }

        [JsonIgnore]
        public Type ActualType { get; set; }

        [JsonProperty("Default")]
        public object DefaultValue { get; set; }

        [JsonProperty("Value")]
        public string StringValue { get; set; }

        public object ActualValue { get; internal set; } = null;

        public object GetActualValue(object value)
        {
            if (value == null)
            {
                UIController.Log($"Style property {Name} has no value!");
                return null;
            }
;
            switch (Type)
            {
                case "string":
                    ActualType = typeof(string);
                    break;
                case "int":
                    ActualType = typeof(long);
                    break;
                case "float":
                    ActualType = typeof(double);
                    break;
                case "bool":
                    ActualType = typeof(bool);
                    break;
                case "point":
                    ActualType = typeof(string);
                    break;
                case "pointF":
                    ActualType = typeof(string);
                    break;
                case "font":
                    ActualType = typeof(string);
                    break;
                case "color":
                    ActualType = typeof(string);
                    break;
                default:
                    UIController.Log($"Style property {Name} has an unknown type '{Type}'");
                    return null;
            }

            if (value.GetType() != ActualType)
            {
                UIController.Log($"Style property {Name} is of type {ActualType} but the value '{value}' is not a {ActualType}, it's a {value.GetType()}.");
                return null;
            }
            
            object actualValue = value;

            if (Type == "int")
            {
                ActualType = typeof(int);
                actualValue = Convert.ToInt32(value);
            }

            if (Type == "float")
            {
                ActualType = typeof(float);
                actualValue = Convert.ToSingle(value, new CultureInfo("en-US"));
            }

            if (Type == "color")
            {
                try
                {
                    ActualType = typeof(Color);
                    return ColorTranslator.FromHtml((string)value);
                }
                catch (Exception ex)
                {
                    UIController.Log($"Style property {Name} is of type color but the value '{value}'is not a valid {typeof(Color)}: {ex}");
                    return false;
                }
            }

            if (Type == "font")
            {
                if (Enum.TryParse((string)value, out GTA.UI.Font actualFont))
                {
                    ActualType = typeof(GTA.UI.Font);
                    return actualFont;
                }
                else
                {
                    UIController.Log($"Style property {Name} is of type font but the value '{value}' is not a valid font name.");
                    return false;
                }
            }

            if (Type == "point")
            {
                string[] parts = ((string)value).Split(',');
                if (parts.Length != 2)
                {
                    UIController.Log($"Style property {Name} is of type point but it's string value '{value}' does not contain two coordinates separated by a '|'.");
                    return null;
                }


                if (int.TryParse(parts[0], out int x) == false)
                {
                    UIController.Log($"Style property {Name} is of type point but it's string value '{value}' contains an invalid x coordinate '{parts[0]}'");
                    return null;
                }

                if (int.TryParse(parts[1], out int y) == false)
                {
                    UIController.Log($"Style property {Name} is of type point but it's string value '{value}' contains an invalid y coordinate '{parts[1]}'");
                    return null;
                }
                ActualType = typeof(Point);
                return new Point(x, y);
            }

            if (Type == "pointF")
            {
                string[] parts = ((string)value).Split(',');
                if (parts.Length != 2)
                {
                    UIController.Log($"Style property {Name} is of type point but it's string value '{value}' does not contain two coordinates separated by a '|'.");
                    return null;
                }


                if (float.TryParse(parts[0], NumberStyles.Any, new CultureInfo("en-US"), out float x) == false)
                {
                    UIController.Log($"Style property {Name} is of type pointF but it's string value '{value}' contains an invalid x coordinate '{parts[0]}'");
                    return null;
                }

                if (float.TryParse(parts[1], NumberStyles.Any, new CultureInfo("en-US"), out float y) == false)
                {
                    UIController.Log($"Style property {Name} is of type pointF but it's string value '{value}' contains an invalid y coordinate '{parts[1]}'");
                    return null;
                }
                ActualType = typeof(PointF);
                return new PointF(x, y);
            }


            return actualValue;
        }
    }
}
