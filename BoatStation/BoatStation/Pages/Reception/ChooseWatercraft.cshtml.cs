using System.Threading.Tasks;
using BoatStation.Data;
using BoatStation.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Reception
{
    public class ChooseWatercraft : PageModel
    {
        private readonly IDbContext _dbContext;

        public ChooseWatercraft(IDbContext context)
        {
            _dbContext = context;
        }

        public SelectResult AvailableWatercrafts { get; set; }
        
        [BindProperty]
        public int ClientId { get; set; }
        
        [BindProperty]
        public int WatercraftId { get; set; }
        
        
        public async Task OnGet(string pondType, int clientId)
        {
            ClientId = clientId;
            AvailableWatercrafts = await _dbContext.ExecuteReaderAsync($"SELECT * FROM get_available_watercraft_by_pond('{pondType}');");
        }
        
        public IActionResult OnPost()
        {
            return RedirectToPage("Briefing", new { clientId = ClientId, watercraftId = WatercraftId });
        }
    }
}