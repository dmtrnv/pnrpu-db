using System;
using System.Threading.Tasks;
using DbLab5.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DbLab5.Pages
{
    public class Operations : PageModel
    {
        private readonly IDbContext _context;

        public Operations(IDbContext context)
        {
            _context = context;
        }
        
        [BindProperty]
        public string NewName { get; set; }
        
        public void OnGet()
        {
            
        }
        
        public async Task<IActionResult> OnPostRenameTable()
        {
            try
            {
                if (!string.IsNullOrEmpty(NewName))
                {
                    await _context.RenameTableAsync(_context.WorkingTableName, NewName);
                    return RedirectToPage("/Tables");
                }
            }
            catch
            {
                return RedirectToPage("/Error");
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveTable()
        {
            try
            {
                await _context.RemoveTableAsync(_context.WorkingTableName);
                return RedirectToPage("/Tables");
            }
            catch
            {
                return RedirectToPage("/Error");
            }
        }
    }
}