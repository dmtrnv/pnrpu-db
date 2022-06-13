using System.Threading.Tasks;
using BoatStation.Data;
using BoatStation.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Reception
{
    public class ContractArchive : PageModel
    {
        private readonly IDbContext _dbContext;

        public ContractArchive(IDbContext context)
        {
            _dbContext = context;
        }
        
        public SelectResult Contracts { get; set; }
        
        public async Task OnGet()
        {
            Contracts = await _dbContext.SelectAllAsync("contract_archive");
        }
    }
}