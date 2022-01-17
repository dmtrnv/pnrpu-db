using System.ComponentModel;
using DbLab5.Misc;

namespace DbLab5.Data
{
    public class CreateTableRowModel
    {
        [DisplayName("Название поля")]
        public string Name { get; set; }
        
        [DisplayName("Тип")]
        public string Type { get; set; }
        
        public bool NotNull { get; set; }
        
        public bool AutoIncrement { get; set; }
        
        public bool PrimaryKey { get; set; }
    }
}