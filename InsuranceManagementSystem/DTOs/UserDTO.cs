using System.ComponentModel.DataAnnotations;

namespace InsuranceManagementSystem.DTOs
{
    public class UserDTO
    {
        public string? Username { get; set; }


        [EmailAddress(ErrorMessage = "Please Enter the valid Email ID")]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
