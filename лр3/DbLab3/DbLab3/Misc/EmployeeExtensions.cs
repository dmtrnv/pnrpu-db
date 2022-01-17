using DbLab3.Models;

namespace DbLab3.Misc
{
    public static class EmployeeExtensions
    {
        public static string GetPropertyValueToString(this Employee employee, string propertyName)
        {
            return employee.GetType().GetProperty(propertyName).GetValue(employee).ToString();
        }
    }
}