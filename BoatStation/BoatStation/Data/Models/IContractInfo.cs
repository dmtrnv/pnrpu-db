namespace BoatStation.Data.Models
{
    public interface IContractInfo
    {
        int ClientId { get; set; }
        int BriefingId { get; set; }
        int InstructorId { get; set; }
        int WatercraftId { get; set; }
        int UseDurationInMinutes { get; set; }
    }
}