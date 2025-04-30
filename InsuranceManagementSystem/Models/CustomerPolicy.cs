using System.ComponentModel.DataAnnotations;

namespace InsuranceManagementSystem.Models
{
    public class CustomerPolicy
    {
        [Key]
        public int CustomerPolicy_ID { get; set; }

        public int Customer_ID { get; set; }
        public Customer? Customer { get; set; }
        public int PolicyID { get; set; }
        public Policy? Policy { get; set; }

    }
}
