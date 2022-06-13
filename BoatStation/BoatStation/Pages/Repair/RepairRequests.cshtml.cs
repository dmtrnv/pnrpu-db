using System.Threading.Tasks;
using BoatStation.Data;
using BoatStation.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Repair
{
    public class RepairRequests : PageModel
    {
        private readonly IDbContext _dbContext;
        
        public SelectResult WatercraftToRepair { get; set; }

        public RepairRequests(IDbContext context)
        {
            _dbContext = context;
        }
        
        public async Task OnGet()
        {
            WatercraftToRepair = await _dbContext.SelectAllAsync("watercraft_to_repair");
        }
        
        public async Task<IActionResult> OnPost(string inventoryNumber)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Error");
            }
            
            await _dbContext.ExecuteNonQueryAsync($"SELECT make_repair('{inventoryNumber}');");
            
            return RedirectToPage("/Repair/RepairRequests");
        }
    }
}