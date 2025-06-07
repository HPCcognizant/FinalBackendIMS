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
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly RenewDate { get; set; }
        public string? PaymentFrequency { get; set; }   
        public decimal PayableAmount { get; set; }

    }
}
