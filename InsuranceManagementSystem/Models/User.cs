using System.ComponentModel.DataAnnotations;

namespace InsuranceManagementSystem.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Username { get; set; }

        [EmailAddress(ErrorMessage = "Please Enter the valid Email ID")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public string? role { get; set; } = "User";

        //navigation property
        public Customer? Customer { get; set; }  
        public Agent? Agent { get; set; }   
        public Admin? Admin { get; set; }
    }
}