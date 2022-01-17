using System;
using System.Threading.Tasks;
using DbLab5.Data;
using DbLab5.Misc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DbLab5.Pages
{
    public class Data : PageModel
    {
        private IDbContext _context;

        public Data(IDbContext context)
        {
            _context = context;
        }
        
        public SelectResult Result { get; set; } = new();
        
        public async Task<IActionResult> OnGet(string tableName)
        {
            try
            {
                Result = await _context.SelectAllAsync(tableName);
            }
            catch
            {
                return RedirectToPage("/Error");
            }
            
            return Page();
        }
    }
}