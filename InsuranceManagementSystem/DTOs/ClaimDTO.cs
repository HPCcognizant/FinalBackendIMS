namespace InsuranceManagementSystem.DTOs
{
    public class ClaimDTO
    {
        public int PolicyID { get; set; }
        public int CustomerID { get; set; }
        public decimal ClaimAmount { get; set; }
        public string? Status { get; set; }
    }
}
