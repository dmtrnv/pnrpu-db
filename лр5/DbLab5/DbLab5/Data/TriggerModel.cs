namespace DbLab5.Data
{
    public class TriggerModel
    {
        public string Name { get; set; }
        public string Time { get; set; }
        public string Event { get; set; }
        public string EventObjectTableName { get; set; }
        public string Statement { get; set; }
    }
}