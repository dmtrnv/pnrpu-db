using System.Collections.Generic;

namespace BoatStation.Data.Models
{
    public class SelectResult
    {
        public List<string> Headers { get; set; } = new();
        
        public List<List<string>> Values { get; set; } = new();
    }
}