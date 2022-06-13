using System.ComponentModel.DataAnnotations;

namespace BoatStation.Data.Models
{
    public class ContractInfo : IContractInfo
    {
        [Required]
        public int ClientId { get; set; }
            
        [Required]
        public int BriefingId { get; set; }
            
        [Required]
        public int InstructorId { get; set; }
            
        [Required]
        public int WatercraftId { get; set; }
            
        [Required]
        public int UseDurationInMinutes { get; set; }
    }
}