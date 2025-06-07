namespace InsuranceManagementSystem.DTOs
{
    public class CustomerPoliciesDTO
    {
        public int Customer_ID { get; set; }
        public int PolicyID { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly RenewDate { get; set; }
        public string? PaymentFrequency { get; set; }
        public decimal PayableAmount { get; set; }
    }

    public class UpdateClaimStatus
    {
        public int ClaimID { get; set; }
        public string? AdminReason { get; set; }
        public string? Status { get; set; }
    }
}
