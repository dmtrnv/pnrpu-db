using System.Threading.Tasks;
using BoatStation.Data;
using BoatStation.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Reception
{
    public class ChoosePond : PageModel
    {
        private readonly IDbContext _dbContext;

        public ChoosePond(IDbContext context)
        {
            _dbContext = context;
        }
        
        public SelectResult AvailablePonds { get; set; } = new();
        
        [BindProperty]
        public int ClientId { get; set; }
        
        [BindProperty]
        public string SelectedPond { get; set; }

        public async Task<IActionResult> OnGet(int clientId)
        {
            ClientId = clientId;
            
            AvailablePonds = await _dbContext.ExecuteReaderAsync("SELECT pond_type.name FROM pond_type;");

            return Page();
        }
        
        public IActionResult OnPostSelectPond()
        {
            return RedirectToPage("ChooseWatercraft", new { pondType = SelectedPond, clientId = ClientId });
        }
    }
}