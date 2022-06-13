using System.Threading.Tasks;
using BoatStation.Data;
using BoatStation.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Reception
{
    public class ActiveContracts : PageModel
    {
        private readonly IDbContext _dbContext;

        public ActiveContracts(IDbContext context)
        {
            _dbContext = context;
        }
        
        public SelectResult Contracts { get; set; }
        
        public async Task OnGet()
        {
            Contracts = await _dbContext.SelectAllAsync("active_contracts");
        }
        
        public async Task<IActionResult> OnPost(string inventoryNumber)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Error");
            }
            
            await _dbContext.ExecuteNonQueryAsync($"SELECT complete_contract('{inventoryNumber}');");
            
            return RedirectToPage("ContractArchive");
        }
    }
}