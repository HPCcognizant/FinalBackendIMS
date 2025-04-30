using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace InsuranceManagementSystem.Models
{
    public class Policy
    {
        [Key]
        public int PolicyID { get; set; }
        public string? Policy_Name { get; set; }
        public decimal PremiumAmount { get; set; }
        public string? CoverageDetails { get; set; }
        public DateOnly ValidityPeriod { get; set; }

        public int AgentID { get; set; }

        //Navigation Property
        public Agent? Agent { get; set; }
        public ICollection<CustomerPolicy>? CustomerPolicies { get; set; }
        public ICollection<Claim>? Claims { get; set; }
    }
}
