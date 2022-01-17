namespace DbLab3.Models
{
    public class Employee
    {
        public int? Id { get; set; }
        
        public string FullName { get; set; }
        
        public string Address { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public int? WorkExperience { get; set; }
        
        public decimal? Salary { get; set; }
    }
}