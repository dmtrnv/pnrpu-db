using System.Threading.Tasks;
using BoatStation.Data;
using BoatStation.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Reception
{
    public class AvailableWatercrafts : PageModel
    {
        public readonly IDbContext _dbContext;

        public AvailableWatercrafts(IDbContext context)
        {
            _dbContext = context;
        }
        
        public SelectResult Watercrafts { get; set; }
        
        public async Task OnGet()
        {
            Watercrafts = await _dbContext.SelectAllAsync("watercraft_base");     
        }
    }
}