using System.Threading.Tasks;
using DbLab5.Data;
using DbLab5.Misc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DbLab5.Pages
{
    public class Procedures : PageModel
    {
        private readonly IDbContext _context;

        public Procedures(IDbContext context)
        {
            _context = context;
        }

        public SelectResult SelectResult { get; set; }
        
        public async Task OnGet()
        {
            SelectResult = await _context.ExecuteReaderAsync
                ($"SELECT routine_name AS PROCEDURE_NAME, routine_definition AS PROCEDURE_DEFINITION, parameter_mode, parameter_name, information_schema.parameters.dtd_identifier AS PARAMETER_TYPE, created FROM information_schema.routines LEFT JOIN information_schema.parameters ON information_schema.parameters.SPECIFIC_NAME = routine_name WHERE information_schema.routines.routine_type = 'PROCEDURE' AND routine_schema = '{_context.DbName}';");
        }
    }
}