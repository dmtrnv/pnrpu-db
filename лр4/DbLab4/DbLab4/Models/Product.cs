namespace DbLab4.Models
{
    public class Product
    {
        public int? Id { get; set; }
        
        public string Name { get; set; }
        
        public decimal? Cost { get; set; }
        
        public int? Count { get; set; }
        
        public int? ManufacturerId { get; set; }
    }
}