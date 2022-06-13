using System.Threading.Tasks;
using BoatStation.Data;
using BoatStation.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Supply
{
    public class OrdersRequests : PageModel
    {
        private readonly IDbContext _dbContext;
        
        public SelectResult WatercraftRequestsToOrder { get; set; }
        
        public OrdersRequests(IDbContext context)
        {
            _dbContext = context;
        }
        
        public async Task OnGet()
        {
            await _dbContext.ExecuteNonQueryAsync("SELECT check_watercraft_operation_period_and_make_request_to_order();");
            
            WatercraftRequestsToOrder = await _dbContext.SelectAllAsync("watercraft_requests_to_order");
        }
        
        public async Task<IActionResult> OnPost(string inventoryNumber)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Error");
            }
            
            await _dbContext.ExecuteNonQueryAsync($"SELECT create_order('{inventoryNumber}');");
            
            return RedirectToPage("/Supply/Orders");
        }
    }
}