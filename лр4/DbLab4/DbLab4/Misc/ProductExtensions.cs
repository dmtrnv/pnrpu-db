using DbLab4.Models;

namespace DbLab4.Misc
{
    public static class ProductExtensions
    {
        public static string GetPropertyValueToString(this Product product, string propertyName)
        {
            return product.GetType().GetProperty(propertyName).GetValue(product).ToString();
        }
    }
}