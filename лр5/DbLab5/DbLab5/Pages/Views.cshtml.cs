using System.Threading.Tasks;
using DbLab5.Data;
using DbLab5.Misc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DbLab5.Pages
{
    public class Views : PageModel
    {
        private readonly IDbContext _context;

        public Views(IDbContext context)
        {
            _context = context;
        }

        public SelectResult SelectResult { get; set; }
        
        public async Task OnGet()
        {
            SelectResult = await _context.ExecuteReaderAsync
                ($"SELECT table_name as VIEW_NAME, create_time FROM information_schema.tables WHERE table_type LIKE 'VIEW' AND table_schema LIKE '{_context.DbName}';");
        }
    }
}