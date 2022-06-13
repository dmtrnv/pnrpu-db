using System.Threading.Tasks;
using BoatStation.Data;
using BoatStation.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Supply
{
    public class Orders : PageModel
    {
        private readonly IDbContext _dbContext;
        
        public SelectResult WatercraftOrders { get; set; }
        
        public Orders(IDbContext context)
        {
            _dbContext = context;
        }

        public async Task OnGet()
        {
            WatercraftOrders = await _dbContext.SelectAllAsync("watercraft_orders");
        }
    }
}