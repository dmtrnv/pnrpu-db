using System.Threading.Tasks;
using BoatStation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Reception
{
    public class ConfirmCreateContract : PageModel
    {
        private readonly IDbContext _dbContext;

        public ConfirmCreateContract(IDbContext context)
        {
            _dbContext = context;
        }
        
        [BindProperty]
        public int ClientId { get; set; }
        
        [BindProperty]
        public int WatercraftId { get; set; }
        
        [BindProperty]
        public int BriefingId { get; set; }
        
        [BindProperty]
        public int UseDurationInMinutes { get; set; }

        public void OnGet(int clientId, int watercraftId, int briefingId)
        {
            ClientId = clientId;
            WatercraftId = watercraftId;
            BriefingId = briefingId;
        }
        
        public async Task<IActionResult> OnPostCreateContract()
        {
            await _dbContext.ExecuteNonQueryAsync($"SELECT create_contract({ClientId}, {WatercraftId}, {BriefingId}, {UseDurationInMinutes});");
            
            return RedirectToPage("ActiveContracts");
        }
    }
}