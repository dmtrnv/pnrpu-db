using System.Collections.Generic;

namespace DbLab5.Misc
{
    public class SelectResult
    {
        public List<string> Headers { get; set; } = new();
        
        public List<List<string>> Values { get; set; } = new();
    }
}