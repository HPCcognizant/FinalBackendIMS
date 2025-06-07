namespace InsuranceManagementSystem.DTOs
{
    public class ClaimDTO
    {
        public int PolicyID { get; set; }
        public int Customer_ID { get; set; }
        public decimal ClaimAmount { get; set; }
        public string? ClaimReason { get; set; }
        public string? AdminReason { get; set; }
    }
}
