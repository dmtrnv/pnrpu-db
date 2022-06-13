using BoatStation.Data;
using BoatStation.Misc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages
{
    public class ChangeUser : PageModel
    {
        private readonly IDbContext _dbContext;
        private readonly IUser _currentUser;

        public ChangeUser(IDbContext dbContext, IUser currentUser)
        {
            _dbContext = dbContext;
            _currentUser = currentUser;
        }
        
        [BindProperty]
        public string Username { get; set; }
        
        [BindProperty]
        public string Password { get; set; }
        
        public void OnGet()
        {
            
        }
        
        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrWhiteSpace(Username) || Password is null)
            {
                return RedirectToPage("/Error");
            }
            
            _dbContext.ChangeUser(Username, Password);
            
            _currentUser.Name = Username;
            _currentUser.Role = Username.Split('_')[0].ToLower();
            
            return RedirectToPage("/Index");
        }
    }
}