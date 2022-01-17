using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DbLab5.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DbLab5.Pages
{
    public class CreateTable : PageModel
    {
        private readonly IDbContext _context;

        public CreateTable(IDbContext context)
        {
            _context = context;
        }
    
        [BindProperty]
        public List<CreateTableRowModel> Rows { get; set; }
        
        [BindProperty]
        [DisplayName("Название таблицы")]
        public string TableName { get; set; }
        
        [BindProperty]
        public int RowsCount { get; set; } = 0;
        
        public void OnGet()
        {
        }
        
        public async Task<IActionResult> OnPost()
        {
            if (Rows.Count != 0)
            {
                try
                {
                    await _context.CreateTableAsync(TableName, Rows);
                    return RedirectToPage("/Tables");
                }
                catch
                {
                    return RedirectToPage("/Error");
                }
            }

            return Page();
        }
    }
}