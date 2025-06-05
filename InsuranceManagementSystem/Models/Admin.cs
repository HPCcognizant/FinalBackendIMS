using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceManagementSystem.Models
{
    public class Admin
    {
        [Key]
        public int Admin_Id { get; set; }
        public string? Admin_Name { get; set; }
        public string? Admin_Contact { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        //navigation property
        public User? User { get; set; }
    }
}
