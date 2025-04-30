namespace InsuranceManagementSystem.DTOs
{
    public class PolicyDTO
    {
        public string? Policy_Name { get; set; }
        public decimal PremiumAmount { get; set; }
        public string? CoverageDetails { get; set; }
        public DateOnly ValidityPeriod { get; set; }
    }
}
