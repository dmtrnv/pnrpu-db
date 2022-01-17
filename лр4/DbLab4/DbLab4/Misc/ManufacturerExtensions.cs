using DbLab4.Models;

namespace DbLab4.Misc
{
    public static class ManufacturerExtensions
    {
        public static string GetPropertyValueToString(this Manufacturer manufacturer, string propertyName)
        {
            return manufacturer.GetType().GetProperty(propertyName).GetValue(manufacturer).ToString();
        }
    }
}