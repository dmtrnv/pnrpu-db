using System.Threading.Tasks;
using BoatStation.Data;
using BoatStation.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Reception
{
    public class ClientsBase : PageModel
    {
        private readonly IDbContext _dbContext;

        public ClientsBase(IDbContext context)
        {
            _dbContext = context;
        }
        
        public SelectResult Clients { get; set; }
        
        public async Task OnGet()
        {
            Clients = await _dbContext.SelectAllAsync("client_base");
        }
    }
}