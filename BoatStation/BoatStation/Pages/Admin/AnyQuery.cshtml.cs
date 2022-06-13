using System;
using System.Threading.Tasks;
using BoatStation.Data;
using BoatStation.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Admin
{
    public class AnyQuery : PageModel
    {
        private readonly IDbContext _context;

        public AnyQuery(IDbContext context)
        {
            _context = context;
        }
        
        [BindProperty]
        public string Query { get; set; } = string.Empty;
        
        public SelectResult SelectResult { get; set; } = new();
        
        public string OtherQueryResult { get; set; } = string.Empty;

        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrEmpty(Query))
            {
                return Page();
            }
            
            if (Query.StartsWith("select", StringComparison.OrdinalIgnoreCase)
                || Query.StartsWith("call", StringComparison.OrdinalIgnoreCase))
            {
                SelectResult = await _context.ExecuteReaderAsync(Query);
            }
            else
            {
                var rowCount = await _context.ExecuteNonQueryAsync(Query);
                OtherQueryResult = $"{rowCount} rows affected by the query";
            }
            
            return Page();
        }
    }
}