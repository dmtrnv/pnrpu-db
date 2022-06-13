using BoatStation.Misc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUser _currentUser;

        public IndexModel(IUser user)
        {
            _currentUser = user;
        }
        
        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(_currentUser.Name))
            {
                return RedirectToPage("/ChangeUser");
            }
            
            return Page();
        }
    }
}