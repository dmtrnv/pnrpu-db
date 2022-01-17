using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbLab5.Data;
using DbLab5.Misc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DbLab5.Pages
{
    public class Triggers : PageModel
    {
        private readonly IDbContext _context;

        public Triggers(IDbContext context)
        {
            _context = context;
        }
        
        [BindProperty]
        public TriggerModel Trigger { get; set; } = new();
        
        public List<TriggerModel> TriggerModels { get; set; } = new();
        
        public async Task OnGet()
        {
            TriggerModels = await _context.GetTriggersAsync();
        }

        public async Task<IActionResult> OnPostRemoveTrigger()
        {
            try
            {
                await _context.RemoveTriggerAsync(Trigger.Name);
                return Page();
            }
            catch
            {
                return RedirectToPage("/Error");
            }
        }
    }
}