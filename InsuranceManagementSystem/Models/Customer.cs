using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace InsuranceManagementSystem.Models
{
    public class Customer
    {
        [Key]
        public int Customer_ID { get; set; }
        public string? Customer_Name { get; set; }
        public string? Customer_Email { get; set; }
        public long Customer_Phone { get; set; }
        public string? Customer_Address { get; set; }

        //Navigation Properties
        public ICollection<CustomerPolicy>? CustomerPolicies { get; set; }
        public ICollection<Claim>? Claims { get; set; }
        public ICollection<Notification>? Notifications { get; set; }   
    }
}
