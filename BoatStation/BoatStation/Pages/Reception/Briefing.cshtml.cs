using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoatStation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Reception
{
    public class Briefing : PageModel
    {
        private readonly IDbContext _dbContext;

        public Briefing(IDbContext context)
        {
            _dbContext = context;
        }
        
        [BindProperty]
        public int ClientId { get; set; }
        
        [BindProperty]
        public int WatercraftId { get; set; }
        
        [BindProperty]
        public string SelectedBriefingType { get; set; }
        
        [BindProperty]
        public string SelectedInstructor { get; set; }
        
        public List<string> BriefingTypes { get; set; }
        
        public List<string> Instructors { get; set; }
        
        public async Task OnGet(int clientId, int watercraftId)
        {
            ClientId = clientId;
            WatercraftId = watercraftId;
            
            BriefingTypes = (await _dbContext.ExecuteReaderAsync("SELECT name FROM briefing_type;"))
                .Values
                .SelectMany(h => h)
                .ToList();
            
            Instructors = (await _dbContext.ExecuteReaderAsync("SELECT * FROM get_instructors_as_strings;"))
                .Values
                .SelectMany(h => h)
                .ToList();
        }
        
        public async Task<IActionResult> OnPostMakeBriefing()
        {
            var instructorId = (int)await _dbContext.ExecuteScalarAsync($"SELECT instructor_id FROM instructor WHERE CONCAT(surname, ' ', name, ' ', patronymic) = '{SelectedInstructor.Split(',')[0]}';");

            var briefingTypeId = (int)await _dbContext.ExecuteScalarAsync($"SELECT type_id FROM briefing_type WHERE name = '{SelectedBriefingType}';");
            var briefingId = (int)await _dbContext.ExecuteScalarAsync($"SELECT make_briefing({briefingTypeId}, {instructorId}, {ClientId});");

            return RedirectToPage("ConfirmCreateContract", new { clientId = ClientId, watercraftId = WatercraftId, briefingId = briefingId });
        }
    }
}