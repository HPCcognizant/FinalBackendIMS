using System.ComponentModel.DataAnnotations;

namespace InsuranceManagementSystem.Models
{
    public class Agent
    {
        [Key]
        public int AgentID { get; set; }
        public string? Agent_Name { get; set; }
        public string? ContactInfo { get; set; }
        public ICollection<Policy>? AssignedPolicies { get; set; }

    }
}
