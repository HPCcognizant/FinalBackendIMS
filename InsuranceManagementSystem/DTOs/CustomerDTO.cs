using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace InsuranceManagementSystem.DTOs
{
    public class CustomerDTO
    {
        [Required(ErrorMessage = "Please Enter the Name")]
        public string? Customer_Name { get; set; }

        [Required(ErrorMessage = "Please Enter the Phone number")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage ="Enter the valid Phone number")]
        public long Customer_Phone { get; set; }
        public string? Customer_Address { get; set; }
    }
}
