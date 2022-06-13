using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BoatStation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BoatStation.Pages.Reception
{
    public class CreateContract : PageModel
    {
        private readonly IDbContext _dbContext;

        public CreateContract(IDbContext context)
        {
            _dbContext = context;
        }
        
        [BindProperty]
        public ClientInfo Client { get; set; }

        public void OnGet()
        {
            
        }
        
        public async Task<IActionResult> OnPostCreateClient()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var clientId = (int)await _dbContext.ExecuteScalarAsync(
                $"SELECT create_client('{Client.SerialNumber}', '{Client.Number}', '{Client.IssuedBy}', '{Client.DivisionCode}'," +
                $" '{Client.IssuedDate}', '{Client.BirthdayDate}', '{Client.Gender}', '{Client.Region}', '{Client.City}', " +
                $"'{Client.Street}', '{Client.HouseNumber}', '{Client.ApartmentNumber}', '{Client.Name}', '{Client.Surname}', '{Client.Patronymic}');");
            
            return RedirectToPage("ChoosePond", new { clientId = clientId });
        }
        
        public class ClientInfo
        {
            [Required(ErrorMessage = "Заполните поле.")]
            [MaxLength(32)]
            public string Name { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [MaxLength(32)]
            public string Surname { get; set; }
                    
            [Required(ErrorMessage = "Заполните поле.")]
            [MaxLength(32)]
            public string Patronymic { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [StringLength(4)]
            [RegularExpression("[0-9]{4}")]
            public string SerialNumber { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [StringLength(6)]
            [RegularExpression("[0-9]{6}")]
            public string Number { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [MaxLength(128)]
            public string IssuedBy { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [StringLength(10)]
            [RegularExpression("[0-9]{4}-[0-9]{2}-[0-9]{2}")]
            public string IssuedDate { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [StringLength(7)]
            [RegularExpression("[0-9]{3}-[0-9]{3}")]
            public string DivisionCode { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [StringLength(10)]
            [RegularExpression("[0-9]{4}-[0-9]{2}-[0-9]{2}")]
            public string BirthdayDate { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [RegularExpression("М|м|Ж|ж")]
            public char Gender { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [MaxLength(64)]
            public string Region { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [MaxLength(64)]
            public string City { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [MaxLength(64)]
            public string Street { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [MaxLength(16)]
            public string HouseNumber { get; set; }
            
            [Required(ErrorMessage = "Заполните поле.")]
            [MaxLength(16)]
            public string ApartmentNumber { get; set; }
        }
    }
}