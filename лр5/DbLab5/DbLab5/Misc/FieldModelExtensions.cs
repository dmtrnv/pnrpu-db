using DbLab5.Data;

namespace DbLab5.Misc
{
    public static class FieldModelExtensions
    {
        public static string GetPropertyValue(this FieldModel field, string propertyName)
        {
            return field.GetType().GetProperty(propertyName).GetValue(field).ToString();
        }
    }
}