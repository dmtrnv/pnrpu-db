using DbLab5.Data;

namespace DbLab5.Misc
{
    public static class TriggerModelExtensions
    {
        public static string GetPropertyValue(this TriggerModel trigger, string propertyName)
        {
            return trigger.GetType().GetProperty(propertyName).GetValue(trigger).ToString();
        }
    }
}