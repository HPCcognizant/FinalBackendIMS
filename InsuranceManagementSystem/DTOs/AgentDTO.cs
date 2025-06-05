using System.ComponentModel.DataAnnotations;

namespace InsuranceManagementSystem.DTOs
{
    public class AgentDTO
    {
        [Required(ErrorMessage = "Please Enter the Name")]
        public string? Agent_Name { get; set; }

        [Required(ErrorMessage = "Please Enter the Phone number")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter the valid Phone number")]
        public string? ContactInfo { get; set; }

    }
}
