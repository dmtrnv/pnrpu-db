using System.Threading.Tasks;
using BoatStation.Data;
using BoatStation.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Reception
{
    public class AvailablePonds : PageModel
    {
        private readonly IDbContext _dbContext;

        public AvailablePonds(IDbContext context)
        {
            _dbContext = context;
        }
        
        public SelectResult Ponds { get; set; }
        
        public async Task OnGet()
        {
            Ponds = await _dbContext.SelectAllAsync("pond_base");
        }
    }
}