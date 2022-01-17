using System.Collections.Generic;
using System.Threading.Tasks;
using DbLab5.Data;
using DbLab5.Misc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DbLab5.Pages
{
    public class Tables : PageModel
    {
        private readonly IDbContext _context;
        
        public List<string> TableNames { get; private set;} = new();

        public Tables(IDbContext context)
        {
            _context = context;
        }

        public async Task OnGet()
        {
            _context.WorkingTableName = string.Empty;
            
            TableNames = await _context.GetTableNamesAsync();
        }
    }
}