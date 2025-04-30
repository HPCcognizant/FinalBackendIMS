using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceManagementSystem.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }

        [ForeignKey("Customer")]
        public int Customer_ID { get; set; }
        public string? Message { get; set; }
        public DateOnly Datasent { get; set; }

        public Customer? Customer { get; set; }
    }
}
