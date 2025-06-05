using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceManagementSystem.Models
{
    public class Agent
    {
        [Key]
        public int AgentID { get; set; }
        public string? Agent_Name { get; set; }

        [Required(ErrorMessage = "Please Enter the Phone number")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter the valid Phone number")]
        public string? ContactInfo { get; set; }
        public ICollection<Policy>? AssignedPolicies { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        //navigation property
        public User? User { get; set; }
    }
}
