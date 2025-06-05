using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace InsuranceManagementSystem.Models
{
    public class Customer
    {
        [Key]
        public int Customer_ID { get; set; }

        [Required(ErrorMessage ="Please Enter the Name")]
        public string? Customer_Name { get; set; }

        [EmailAddress(ErrorMessage ="Please Enter the valid Email ID")]
        public string? Customer_Email { get; set; }

        [Required(ErrorMessage ="Please Enter the Phone number")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter the valid Phone number")]
        public long Customer_Phone { get; set; }
        public string? Customer_Address { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        //Navigation Properties

        public User? User { get; set; }
        public ICollection<CustomerPolicy>? CustomerPolicies { get; set; }
        public ICollection<Claim>? Claims { get; set; }
        public ICollection<Notification>? Notifications { get; set; }   
    }
}
